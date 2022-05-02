using AutoMapper;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Mappings;

public class WorkoutAreaSessionCountConverter : ITypeConverter<WorkoutAreaSessionCount, WorkoutAreaSessionModel>
{
    public WorkoutAreaSessionModel Convert(WorkoutAreaSessionCount source, WorkoutAreaSessionModel destination, ResolutionContext context)
    {
        var startTime = source.Session.StartTime.ToString("hh\\:mm");
        var endTime = source.Session.EndTime.ToString("hh\\:mm");
        
        return new WorkoutAreaSessionModel
        {
            AreaId = source.Area.WorkoutAreaId,
            SessionId = source.Session.WorkoutSessionId,
            Display = $"{startTime} - {endTime}",
            Enabled = source.Count > 0
        };
    }
}