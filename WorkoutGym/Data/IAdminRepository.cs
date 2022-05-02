namespace WorkoutGym.Data;

public interface IAdminRepository
{
    Task<IEnumerable<MemberSession>> GetMemberSessionsByDateAsync(DateTime date);
    Task<IEnumerable<MemberSession>> GetMemberSessionsByDateRangeAsync(DateTime startDate, DateTime endDate);
}