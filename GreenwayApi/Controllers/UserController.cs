using GreenwayApi.Data;
using GreenwayApi.DTOs.User;
using GreenwayApi.Mapper;
using GreenwayApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GreenwayApi.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    
    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("{id:guid}")]
    public IActionResult FindById([FromRoute] Guid id)
    {
        var userFound = _dbContext.Find<User>(id);
        if (userFound == null)
        {
            return NotFound();
        }
        return Ok(userFound.UserToResponseDto());
    }


    [HttpPost(Name = "Create User")]
    public IActionResult Save([FromBody] UserRequestDto user)
    {
        var userToSave = user.UserRequestDtoToUser();
        _dbContext.Users.Add(userToSave);

        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            return new StatusCodeResult(500);
        }

        return CreatedAtAction(nameof(FindById), new {id = userToSave.Id}, userToSave.UserToResponseDto());
    }
    
}