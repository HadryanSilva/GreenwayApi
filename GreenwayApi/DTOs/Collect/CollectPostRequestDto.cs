using GreenwayApi.Model;

namespace GreenwayApi.DTOs.Collect;

public class CollectPostRequestDto
{
    public WasteType WasteType { get; set; }

    public Guid? UserId { get; set; }
}