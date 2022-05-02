namespace WorkoutGym.Data;

public interface IMemberRepository
{
   Task<IEnumerable<WorkoutArea>> GetWorkoutAreasAsync();
   Task<IEnumerable<WorkoutAreaSessionCount>> GetWorkoutAreaSessionCountsByDateAsync(int workoutAreaId, DateTime date);
   Task<MemberSession> CreateMemberSessionBookingAsync(MemberSessionBooking booking);
   Task<IEnumerable<MemberSession>> GetMemberSessionsByDateAsync(string userId, DateTime date);
   Task<IEnumerable<MemberSession>> GetMemberSessionsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
   Task<BookingValidityCheckResult> MemberSessionBookingValidityCheckAsync(MemberSessionBooking booking);
}