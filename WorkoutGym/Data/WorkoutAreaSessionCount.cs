namespace WorkoutGym.Data;

public class WorkoutAreaSessionCount
{
    public WorkoutArea Area { get; set; } = null!;
    public WorkoutSession Session { get; set; } = null!;
    public int Count { get; set; }
}