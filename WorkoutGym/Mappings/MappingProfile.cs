using AutoMapper;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<UserRegistrationModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opts => opts.MapFrom(o => o.Email));
        this.CreateMap<WorkoutAreaSessionCount, WorkoutAreaSessionModel>().ConvertUsing<WorkoutAreaSessionCountConverter>();
        this.CreateMap<WorkoutArea, WorkoutAreaModel>().ConvertUsing<WorkoutAreaConverter>();
        this.CreateMap<MemberSessionBookingModel, MemberSessionBooking>();
        this.CreateMap<MemberSession, MemberSessionModel>().ConvertUsing<MemberSessionConverter>();
        this.CreateMap<BookingValidityCheckResult, BookingValidityCheckResultModel>()
            .ConvertUsing<BookingValidityCheckResultConverter>();
    }
}