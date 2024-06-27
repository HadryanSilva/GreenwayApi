using GreenwayApi.Data;
using GreenwayApi.DTOs.User;
using GreenwayApi.Model;
using GreenwayApi.Model.AutenticatorModel;
using GreenwayApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace GreenwayApi.Controllers;

[Authorize]
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AuthController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [AllowAnonymous]
    [HttpPost("v1/register")]
    public async Task<IActionResult> Register([FromServices] ApplicationDbContext context, [FromBody] UserRequestDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var exists = await context
            .Users
            .AnyAsync(user => user.Username == model.Username || user.Email == model.Email);

        if (exists)
        {
            return BadRequest("This user already exists.");
        }

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            Role = model.Role,
            Password = PasswordHasher.Hash(model.Password),
        };

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Created(string.Empty, user); ;
        }
        catch
        {
            return StatusCode(500, "Internal Error");
        }
    }

    [AllowAnonymous]
    [HttpPost("v1/login")]
    public async Task<IActionResult> Login(
      [FromBody] LoginViewModel model,
      [FromServices] ApplicationDbContext context,
      [FromServices] ITokenService tokenService)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values);

        var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, "User or password invalid");

        if (!PasswordHasher.Verify(user.Password, model.Password))
            return StatusCode(401, "User or password invalid");

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(token);
        }
        catch
        {
            return StatusCode(500, "Internal Error");
        }
    }


}
