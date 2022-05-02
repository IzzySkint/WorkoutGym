namespace WorkoutGym.Models;

public class BookingValidityCheckResultModel
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = null!;
}