namespace WorkoutGym.Models;

public class MemberSessionModel
{
    public long SessionId { get; set; }
    public string Member { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public string Area { get; set; } = null!;
}