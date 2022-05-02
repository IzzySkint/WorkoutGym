using AutoMapper;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Mappings;

public class WorkoutAreaConverter : ITypeConverter<WorkoutArea, WorkoutAreaModel>
{
    public WorkoutAreaModel Convert(WorkoutArea source, WorkoutAreaModel destination, ResolutionContext context)
    {
        return new WorkoutAreaModel
        {
            Id = source.WorkoutAreaId,
            Name = source.Name
        };
    }
}