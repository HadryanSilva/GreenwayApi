using GreenwayApi.Data;
using GreenwayApi.DTOs.Collect;
using GreenwayApi.Mapper;
using GreenwayApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenwayApi.Controllers;

[Route("api/collects")]
[ApiController]
public class CollectController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    
    public CollectController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet(Name = "Find all collects (Required Admin)")]
    public async Task<IActionResult> FindAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var collects = await _dbContext.Collects
            .OrderBy(i => i.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();
        return Ok(collects);
    }
    
    [HttpGet("user-collects", Name = "Find all collects by user")]
    public async Task<IActionResult> FindAllByUser([FromQuery] Guid userId, [FromQuery] int pageNumber, 
        [FromQuery] int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var collects = await _dbContext.Collects
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();
        return Ok(collects);
    }
    
    [HttpGet("{id:int}", Name = "Find collect by id")]
    public IActionResult FindById([FromRoute] int id)
    {
        var collect = _dbContext.Collects.Find(id);

        if (collect == null)
        {
            return NotFound();
        }
        return Ok(collect);
    }

    [HttpPost(Name = "Create collect")]
    public IActionResult Save([FromBody] CollectRequestDto collect)
    {
        var collectToSave = collect.CollectRequestDtoToCollect();
        _dbContext.Collects.Add(collectToSave);
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            return new StatusCodeResult(500);
        }
        return CreatedAtAction(nameof(FindById), new {id = collectToSave.Id}, 
            collectToSave.CollectToResponseDto());
    }
    
}