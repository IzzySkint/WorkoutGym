namespace WorkoutGym.Data;

public enum BookingValidationResultType
{
    ValidResult = 0,
    InvalidSessionResult = 1,
    InvalidSessionExpiredResult = 2,
    InvalidConsecutiveSessionsResult = 3,
    InvalidSessionBookedResult = 4,
    InvalidSessionMaximumReachedResult = 5
}