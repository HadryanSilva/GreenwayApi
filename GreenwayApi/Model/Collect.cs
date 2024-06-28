using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenwayApi.Model;

[Table("collects")]
public class Collect
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)] 
    [Column("waste_type")]
    public WasteType WasteType { get; set; }
    
    [Column("schedule_date")]
    public DateTime ScheduleDate { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }
}