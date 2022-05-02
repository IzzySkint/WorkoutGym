namespace WorkoutGym.Reports;

public interface IReportsService
{
    Task<byte[]> GenerateReportAsync(IHtmlTemplate template);
}