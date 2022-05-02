using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkoutGym.Models;

namespace WorkoutGym.Data;

public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger _logger;
    
    public MemberRepository(ApplicationDbContext dbContext, ILogger<MemberRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<WorkoutArea>> GetWorkoutAreasAsync()
    {
        try
        {
          var result = await _dbContext.WorkoutAreas.Select(e => e).ToListAsync();
          
          return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving WorkoutAreas");
            throw new RepositoryException(nameof(GetWorkoutAreasAsync), e);
        }
    }

    public async Task<IEnumerable<WorkoutAreaSessionCount>> GetWorkoutAreaSessionCountsByDateAsync(int workoutAreaId,
        DateTime date)
    {
        try
        {
            var workoutArea = _dbContext.WorkoutAreas.FirstOrDefault(e => e.WorkoutAreaId == workoutAreaId);

            if (workoutArea == null)
            {
                throw new ArgumentException(nameof(workoutAreaId));
            }

            var localDate = date.ToLocalTime();
            
            var sessionsInUse = _dbContext.MemberSessions
                .Where(e => e.WorkoutAreaId == workoutAreaId && e.Date.Year == localDate.Year && e.Date.Month == localDate.Month && e.Date.Day == localDate.Day)
                .ToList()
                .GroupBy(e => e.WorkoutSessionId)
                .Select(g => new
                {
                    WorkoutSessionId = g.Key,
                    SessionCount = g.Count()
                }).ToList();

            sessionsInUse.Add(new {WorkoutSessionId = 1, SessionCount = 2});
            
            var workoutAreaSessionCounts = await _dbContext.WorkoutSessions
                .Select(e => new WorkoutAreaSessionCount
                    {
                        Area = workoutArea,
                        Session = e,
                    }
                ).ToListAsync();

            foreach (var wasc in workoutAreaSessionCounts)
            {
                wasc.Count = workoutArea.NumberSessions - sessionsInUse
                    .Where(g => g.WorkoutSessionId == wasc.Session.WorkoutSessionId)
                    .Select(g => g.SessionCount)
                    .FirstOrDefault();
            }
            
            return workoutAreaSessionCounts;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error retrieving WorkoutAreaSessions");
            throw new RepositoryException(nameof(GetWorkoutAreaSessionCountsByDateAsync), e);
        }
    }

    public async Task<MemberSession> CreateMemberSessionBookingAsync(MemberSessionBooking booking)
    {
        var session = new MemberSession
        {
            Date = booking.Date.ToLocalTime(),
            UserId = booking.UserId,
            WorkoutAreaId = booking.WorkoutAreaId,
            WorkoutSessionId = booking.WorkoutSessionId
        };

        try
        {
            _dbContext.MemberSessions.Add(session);
            await _dbContext.SaveChangesAsync();
            
            var result = _dbContext.MemberSessions
                .Include(e => e.WorkoutArea)
                .Include(e => e.WorkoutSession)
                .Include(e => e.User)
                .Where(e => e.MemberSessionId == session.MemberSessionId)
                .Select(e => e).FirstOrDefault();

            return result ?? new MemberSession();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating MemberSessionBooking");
            throw new RepositoryException(nameof(CreateMemberSessionBookingAsync), e);
        }
    }

    public async Task<IEnumerable<MemberSession>> GetMemberSessionsByDateAsync(string userId, DateTime date)
    {
        try
        {
            var localDate = date.ToLocalTime();
            
            var result = await _dbContext.MemberSessions
                .Include(e => e.WorkoutArea)
                .Include(e => e.WorkoutSession)
                .Include(e => e.User)
                .Where(e => e.UserId == userId && e.Date.Year== localDate.Year && e.Date.Month == localDate.Month && e.Date.Day == localDate.Day)
                .OrderBy(e => e.Date)
                .ThenBy(e => e.WorkoutSessionId)
                .ThenBy(e => e.WorkoutAreaId)
                .ToListAsync();
            
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving MemberSessions by date");
            throw new RepositoryException(nameof(GetMemberSessionsByDateAsync), e);
        }
    }

    public async Task<IEnumerable<MemberSession>> GetMemberSessionsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Invalid start date");
        }

        var startLocalDate = startDate.ToLocalTime();
        var endLocalDate = endDate.ToLocalTime();
        
        try
        {
            var result = await _dbContext.MemberSessions
                .Include(e => e.WorkoutArea)
                .Include(e => e.WorkoutSession)
                .Include(e => e.User)
                .Where(e => e.UserId == userId && e.Date >= startLocalDate && e.Date <= endLocalDate)
                .OrderBy(e => e.Date)
                .ThenBy(e => e.WorkoutSessionId)
                .ThenBy(e => e.WorkoutAreaId)
                .ToListAsync();

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving MemberSessions by date range");
            throw new RepositoryException(nameof(GetMemberSessionsByDateRangeAsync), e);
        }
    }

    public async Task<BookingValidityCheckResult> MemberSessionBookingValidityCheckAsync(MemberSessionBooking booking)
    {
        try
        {
            var workoutSession =
                _dbContext.WorkoutSessions.FirstOrDefault(e => e.WorkoutSessionId == booking.WorkoutSessionId);
            
            if (workoutSession == null)
            {
                return new BookingValidityCheckResult
                {
                    IsValid = false,
                    ResultType = BookingValidationResultType.InvalidSessionResult
                };
            }

            var sessionBooked = _dbContext.MemberSessions.FirstOrDefault(e =>
                e.UserId == booking.UserId && e.Date == booking.Date.ToLocalTime() &&
                e.WorkoutSessionId == booking.WorkoutSessionId);

            if (sessionBooked != null)
            {
                return new BookingValidityCheckResult
                {
                    IsValid = false,
                    ResultType = BookingValidationResultType.InvalidSessionBookedResult
                };
            }

            var sessionsForDate = await _dbContext.MemberSessions
                .Where(e => e.UserId == booking.UserId && e.Date == booking.Date.ToLocalTime()).ToListAsync();

            if (sessionsForDate.Count == 6)
            {
                return new BookingValidityCheckResult
                {
                    IsValid = false,
                    ResultType = BookingValidationResultType.InvalidSessionMaximumReachedResult
                };
            }
            
            var localDate = booking.Date.ToLocalTime();
            var currentDate = DateTime.Now;

            if (localDate.Year == currentDate.Year && localDate.Month == currentDate.Month &&
                localDate.Day == currentDate.Day)
            {
                if (workoutSession.StartTime < currentDate.TimeOfDay)
                {
                    return new BookingValidityCheckResult
                    {
                        IsValid = false,
                        ResultType = BookingValidationResultType.InvalidSessionExpiredResult
                    };
                }
            }

            var validityCheckResult = await CheckConsecutiveSessions(booking);
            
            return validityCheckResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error performing MemberSession validity check");
            throw new RepositoryException(nameof(MemberSessionBookingValidityCheckAsync), e);
        }
    }

    private async Task<BookingValidityCheckResult> CheckConsecutiveSessions(MemberSessionBooking booking)
    {
        int oneBehind = booking.WorkoutSessionId - 1;
        int twoBehind = booking.WorkoutSessionId - 2;
        int oneInFront = booking.WorkoutSessionId + 1;
        int twoInFront = booking.WorkoutSessionId + 2;
        
        var consecutiveSessionsBehind = await _dbContext.MemberSessions
            .Include(e => e.WorkoutSession)
            .Where(e => e.UserId == booking.UserId && (e.WorkoutSessionId == oneBehind || e.WorkoutSessionId == twoBehind) && e.WorkoutAreaId == booking.WorkoutAreaId && e.Date == booking.Date.ToLocalTime())
            .ToListAsync();

        if (consecutiveSessionsBehind.Count() == 2)
        {
            return new BookingValidityCheckResult
            {
                IsValid = false,
                ResultType = BookingValidationResultType.InvalidConsecutiveSessionsResult,
                ConsecutiveSessions = consecutiveSessionsBehind.Select(e => e.WorkoutSession).ToList()
            };
        }
        
        var consecutiveSessionsInFront = await _dbContext.MemberSessions
            .Include(e => e.WorkoutSession)
            .Where(e => e.UserId == booking.UserId && (e.WorkoutSessionId == oneInFront || e.WorkoutSessionId == twoInFront) && e.WorkoutAreaId == booking.WorkoutAreaId && e.Date == booking.Date.ToLocalTime())
            .ToListAsync();

        if (consecutiveSessionsInFront.Count() == 2)
        {
            return new BookingValidityCheckResult
            {
                IsValid = false,
                ResultType = BookingValidationResultType.InvalidConsecutiveSessionsResult,
                ConsecutiveSessions = consecutiveSessionsInFront.Select(e => e.WorkoutSession).ToList()
            }; 
        }

        return new BookingValidityCheckResult
        {
            IsValid = true,
            ResultType = BookingValidationResultType.ValidResult,
            ConsecutiveSessions = new List<WorkoutSession>()
        };
    }
    
    
}