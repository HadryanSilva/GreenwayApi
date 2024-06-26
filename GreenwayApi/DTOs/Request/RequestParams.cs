namespace GreenwayApi.DTOs.Request;

public class RequestParams
{
    public Guid UserId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}