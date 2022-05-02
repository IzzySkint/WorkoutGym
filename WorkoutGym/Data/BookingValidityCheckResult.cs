namespace WorkoutGym.Data;

public class BookingValidityCheckResult
{
    public bool IsValid { get; set; }
    public BookingValidationResultType ResultType { get; set; }
    public IEnumerable<WorkoutSession> ConsecutiveSessions { get; set; } = null!;
}