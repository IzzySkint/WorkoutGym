using System.Runtime.Serialization;

namespace WorkoutGym.Reports;

[Serializable]
public class ReportGenerationException : Exception
{
    public ReportGenerationException()
        : base()
    {
        
    }

    public ReportGenerationException(string message)
        : base(message)
    {
        
    }

    public ReportGenerationException(string message, Exception innerException)
        : base(message, innerException)
    {
        
    }

    protected ReportGenerationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        
    }
}