using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutGym.Data;

[Table("MemberSession")]
public class MemberSession
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long MemberSessionId { get; set; }
    [ForeignKey("WorkoutArea")]
    public int WorkoutAreaId { get; set; }
    [ForeignKey("WorkoutSession")]
    public int WorkoutSessionId { get; set; }
    public DateTime Date { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; } = null!;

    public WorkoutArea WorkoutArea { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public WorkoutSession WorkoutSession { get; set; } = null!;
}