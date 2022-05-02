using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WorkoutGym.Data;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MinLength(50)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MinLength(80)]
    public string LastName { get; set; } = null!;
}