using GreenwayApi.Model;

namespace GreenwayApi.DTOs.User;

public class UserResponseDto
{
    public Guid Id { get; set; } = Guid.Empty;
    
    public string Username { get; set; }
    
    public string Email { get; set; }

    public UserRole Role { get; set; }
    
    public ICollection<Model.Collect>? Collects { get; set; }
}