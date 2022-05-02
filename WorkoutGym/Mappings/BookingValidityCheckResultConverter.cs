using AutoMapper;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Mappings;

public class BookingValidityCheckResultConverter : ITypeConverter<BookingValidityCheckResult, BookingValidityCheckResultModel>
{
    public BookingValidityCheckResultModel Convert(BookingValidityCheckResult source, BookingValidityCheckResultModel destination,
        ResolutionContext context)
    {
        if (!source.IsValid)
        {
            string message = string.Empty;

            if (source.ResultType == BookingValidationResultType.InvalidSessionResult)
            {
                message = "Invalid session.";
            }

            if (source.ResultType == BookingValidationResultType.InvalidSessionBookedResult)
            {
                message = "Session already booked for this time.";
            }

            if (source.ResultType == BookingValidationResultType.InvalidSessionMaximumReachedResult)
            {
                message = "Maximum number of sessions booked.";
            }
            
            if (source.ResultType == BookingValidationResultType.InvalidSessionExpiredResult)
            {
                message = "The session you have chosen has expired.";
            }

            if (source.ResultType == BookingValidationResultType.InvalidConsecutiveSessionsResult)
            {
                var sessions = source.ConsecutiveSessions.Select(e => e).ToList();
                var startTime1 = sessions[0].StartTime.ToString(@"hh\:mm");
                var endTime1 = sessions[0].EndTime.ToString(@"hh\:mm");
                var startTime2 = sessions[1].StartTime.ToString(@"hh\:mm");
                var endTime2 = sessions[1].EndTime.ToString(@"hh\:mm");
                
                message =
                    $"Cannot book session because your already have two consecutive sessions booked. {startTime1} - {endTime1} and {startTime2} - {endTime2}";

            }
            
            return new BookingValidityCheckResultModel
            {
                IsValid = false,
                Message = message
            };
        }
        else
        {
            return new BookingValidityCheckResultModel
            {
                IsValid = true,
                Message = "Session is available"
            };
        }
    }
}