using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace WorkoutGym.Reports;

public class ReportsService : IReportsService
{
    private readonly ILogger _logger;

    public ReportsService(ILogger<ReportsService> logger)
    {
        this._logger = logger;
    }
    
    public async Task<byte[]> GenerateReportAsync(IHtmlTemplate template)
    {
        try
        {
            byte[] report = null;
            await template.BuildAsync();
            StringReader sr = new StringReader(template.ToHtmlString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            HTMLWorker htmlParser = new HTMLWorker(pdfDoc);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                pdfDoc.Open();
                
                htmlParser.Parse(sr);
                pdfDoc.Close();

                report = ms.ToArray();
                ms.Close();
            }

            return report;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error generating report");
            throw new ReportGenerationException("Error generating report");
        }
    }
}