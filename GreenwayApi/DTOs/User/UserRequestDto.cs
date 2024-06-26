using GreenwayApi.Model;
using GreenwayApi.Model.AutenticatorModel;

namespace GreenwayApi.DTOs.User;

public class UserRequestDto
{
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }

    public UserRole Role { get; set; }
}