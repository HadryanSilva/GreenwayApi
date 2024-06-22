using GreenwayApi.Model;

namespace GreenwayApi.DTOs.Collect;

public class CollectResponseDto
{
    public int Id { get; set; }
    
    public WasteType WasteType { get; set; }
    
    public DateTime ScheduleDate { get; set; } = DateTime.Today.AddDays(3);

    public Guid? UserId { get; set; }
    
}