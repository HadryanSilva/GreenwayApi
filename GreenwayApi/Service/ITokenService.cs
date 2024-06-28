using GreenwayApi.Model;

namespace GreenwayApi.Service;

public interface ITokenService
{
    string GenerateToken(User user);

    Dictionary<string, string> GetUserDataFromToken(string token);
}
