using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutGym.Data;

[Table("WorkoutArea")]
public class WorkoutArea
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WorkoutAreaId { get; set; }
    [Required]
    [MinLength(50)]
    public string Name { get; set; } = null!;

    public int NumberSessions { get; set; }
}