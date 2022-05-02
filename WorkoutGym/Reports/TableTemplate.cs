using System.Text;

namespace WorkoutGym.Reports;

public abstract class TableTemplate : IHtmlTemplate
{
    private readonly StringBuilder _htmlStringBuilder;

    protected TableTemplate()
    {
        this._htmlStringBuilder = new StringBuilder();
    }
    
    public async Task BuildAsync()
    {
        this._htmlStringBuilder.Clear();
        await Task.Run(() => this.BuildHead());
        await Task.Run(() => this.BuildBody());
    }

    protected abstract string GetTitle();
    protected abstract string GetHeading();
    protected abstract string GetTableHeaders();
    protected abstract string GetTableBody();

    private void BuildHead()
    {
        this._htmlStringBuilder.AppendLine("<html>");
        this._htmlStringBuilder.AppendLine("<head>");
        this._htmlStringBuilder.AppendLine("<title>");
        this._htmlStringBuilder.AppendLine(GetTitle());
        this._htmlStringBuilder.AppendLine("</title>");
        this._htmlStringBuilder.AppendLine("</head>");
    }

    private void BuildBody()
    {
        this._htmlStringBuilder.AppendLine("<body>");
        this._htmlStringBuilder.AppendLine($"<h3>{GetHeading()}</h3>");
        this._htmlStringBuilder.AppendLine("<table>");
        this._htmlStringBuilder.AppendLine(GetTableHeaders());
        this._htmlStringBuilder.AppendLine(GetTableBody());
        this._htmlStringBuilder.AppendLine("</table>");
        this._htmlStringBuilder.AppendLine("</html>");
    }
    
    public string ToHtmlString()
    {
        return this._htmlStringBuilder.ToString();
    }
}