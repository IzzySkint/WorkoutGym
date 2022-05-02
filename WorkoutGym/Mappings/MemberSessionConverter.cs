using AutoMapper;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Mappings;

public class MemberSessionConverter : ITypeConverter<MemberSession, MemberSessionModel>
{
    public MemberSessionModel Convert(MemberSession source, MemberSessionModel destination, ResolutionContext context)
    {
        return new MemberSessionModel
        {
            SessionId = source.MemberSessionId,
            Member = $"{source.User.FirstName} {source.User.LastName}",
            Email = source.User.Email,
            Date = source.Date.ToString("yyyy-MM-dd"),
            Area = source.WorkoutArea.Name,
            StartTime = source.WorkoutSession.StartTime.ToString(@"hh\:mm"),
            EndTime = source.WorkoutSession.EndTime.ToString(@"hh\:mm")
        };
    }
}