using System.Text;
using WorkoutGym.Models;

namespace WorkoutGym.Reports;

public class DailyTimeTableTemplate : TableTemplate
{
    private readonly IEnumerable<MemberSessionModel> _data;
    
    public DailyTimeTableTemplate(IEnumerable<MemberSessionModel> data)
        : base()
    {
        this._data = data;
    }
    
    protected override string GetTitle()
    {
        return "Daily Time Table";
    }

    protected override string GetHeading()
    {
        return $"{DateTime.Now.ToString("yyyy-MM-dd")}";
    }

    protected override string GetTableHeaders()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Session</td>");
        sb.AppendLine("<td>Workout Area</td>");
        sb.AppendLine("<td>Member</td>");
        sb.AppendLine("</tr>");

        return sb.ToString();
    }

    protected override string GetTableBody()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var item in this._data)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{item.StartTime} - {item.EndTime}</td>");
            sb.AppendLine($"<td>{item.Area}</td>");
            sb.AppendLine($"<td>{item.Member}</td>");
            sb.AppendLine($"</tr>");
        }

        return sb.ToString();
    }
}