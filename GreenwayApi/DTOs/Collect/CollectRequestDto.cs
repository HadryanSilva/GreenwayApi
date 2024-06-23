using GreenwayApi.Model;

namespace GreenwayApi.DTOs.Collect;

public class CollectRequestDto
{
    public WasteType WasteType { get; set; }
    
    public DateTime ScheduleDate { get; set; } = DateTime.Today.AddDays(3);

    public Guid? UserId { get; set; }
}