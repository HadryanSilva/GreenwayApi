using System.Diagnostics;
using GreenwayApi.DTOs.Collect;
using GreenwayApi.DTOs.Request;
using GreenwayApi.Exceptions;
using GreenwayApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenwayApi.Controllers;

[Authorize]
[Route("api/collects")]
[ApiController]
public class CollectController : ControllerBase
{
    private readonly CollectService _collectService;
    private readonly TokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CollectController(CollectService collectService, 
        TokenService tokenService, 
        IHttpContextAccessor httpContextAccessor)
    {
        _collectService = collectService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet(Name = "Find all collects (Required Admin)")]
    public async Task<IActionResult> FindAll([FromBody] RequestParams parameters)
    {
        var collects = await _collectService.FindAll(parameters);
        return Ok(collects);
    }
    
    [HttpGet("user-collects", Name = "Find all collects by user")]
    public async Task<IActionResult> FindAllByUser([FromBody] RequestParams parameters)
    {
        var collects = await _collectService.FindAllByUser(parameters);
        return Ok(collects);
    }
    
    [HttpGet("{id:int}", Name = "Find collect by id")]
    public IActionResult FindById([FromRoute] int id)
    {
        try
        {
            var collect = _collectService.FindById(id, GetClaims());
            return Ok(collect);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        } catch (NotAuthorizedException e)
        {
            return Unauthorized(e.Message);
        }
        
    }

    [HttpPost(Name = "Create collect")]
    public IActionResult Save([FromBody] CollectGetRequestDto collectGet)
    {
        var collectSaved = _collectService.Save(collectGet);
        return CreatedAtAction(nameof(FindById), new {id = collectSaved.Id}, 
            collectSaved);
    }
    
    [HttpPut(Name = "Update collect")]
    public IActionResult Update([FromBody] CollectPutRequestDto collectPut)
    {
        try
        {
            var collectUpdated = _collectService.Update(collectPut, GetClaims());
            return Ok(collectUpdated);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (NotAuthorizedException e)
        {
            return Unauthorized(e.Message);
        }
    }
    
    [HttpDelete("{id:int}", Name = "Delete collect")]
    public IActionResult Delete([FromRoute] int id)
    {
        try
        {
            _collectService.Delete(id, GetClaims());
            return NoContent();
        } catch (NotFoundException e)
        {
            return NotFound(e.Message);
        } catch (NotAuthorizedException e)
        {
            return Unauthorized(e.Message);
        }
    }

    private Dictionary<string, string> GetClaims()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        return _tokenService.GetUserDataFromToken(token.ToString().Substring("Bearer ".Length));
    }
    
}