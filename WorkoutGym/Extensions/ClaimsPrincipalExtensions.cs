using System.Security.Claims;

namespace WorkoutGym.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        string userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId;
    }
}