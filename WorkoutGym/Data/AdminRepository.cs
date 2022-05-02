using Microsoft.EntityFrameworkCore;

namespace WorkoutGym.Data;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger _logger;
    
    public AdminRepository(ApplicationDbContext dbContext, ILogger<AdminRepository> logger)
    {
        this._dbContext = dbContext;
        this._logger = logger;
    }

    public async Task<IEnumerable<MemberSession>> GetMemberSessionsByDateAsync(DateTime date)
    {
        try
        {
            var localDate = date.ToLocalTime();
            
            var result = await _dbContext.MemberSessions
                .Include(e => e.WorkoutArea)
                .Include(e => e.WorkoutSession)
                .Include(e => e.User)
                .Where(e => e.Date.Year == localDate.Year && e.Date.Month == localDate.Month && e.Date.Day == localDate.Day)
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

    public async Task<IEnumerable<MemberSession>> GetMemberSessionsByDateRangeAsync(DateTime startDate, DateTime endDate)
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
                .Where(e => e.Date >= startLocalDate && e.Date <= endLocalDate)
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
}