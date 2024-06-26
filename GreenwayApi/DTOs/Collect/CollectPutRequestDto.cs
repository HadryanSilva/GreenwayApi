using System.Security.Claims;
using GreenwayApi.Model;

namespace GreenwayApi.DTOs.Collect;

public class CollectPutRequestDto
{
    public int Id { get; set; }
    
    public WasteType WasteType { get; set; }
}