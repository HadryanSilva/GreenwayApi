using GreenwayApi.Model;
using System.Security.Claims;

namespace GreenwayApi.Extensions
{
    public static class RoleClaimExtention
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            return result;
        }
    }
}
