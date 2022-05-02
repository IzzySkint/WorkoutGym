namespace WorkoutGym.Models;

public class WorkoutAreaSessionModel
{
    public int AreaId { get; set; }
    public int SessionId { get; set; }
    public string Display { get; set; } = null!;
    public bool Enabled { get; set; }
}