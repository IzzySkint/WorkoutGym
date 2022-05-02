namespace WorkoutGym.Reports;

public interface IHtmlTemplate
{
    Task BuildAsync();
    string ToHtmlString();
}