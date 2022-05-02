namespace WorkoutGym.Data;

public class MemberSessionBooking
{
    public string UserId { get; set; } = null!;
    public DateTime Date { get; set; }
    public int WorkoutAreaId { get; set; }
    public int WorkoutSessionId { get; set; }
    
}