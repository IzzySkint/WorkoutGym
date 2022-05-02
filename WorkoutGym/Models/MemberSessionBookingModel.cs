namespace WorkoutGym.Models;

public class MemberSessionBookingModel
{
    public string UserId { get; set; } = null!;
    public DateTime Date { get; set; }
    public int WorkoutAreaId { get; set; }
    public int WorkoutSessionId { get; set; }
}