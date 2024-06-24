using GreenwayApi.Data;
using GreenwayApi.DTOs.Collect;
using GreenwayApi.Mapper;
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
    
    [HttpGet(Name = "Find all collects  (Required Admin)")]
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
    
    [HttpGet("user-collects", Name = "Find all collects for a user")]
    public async Task<IActionResult> FindAllCollectsByUser([FromQuery] Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
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
        var collectFound = _dbContext.Collects.Find(id);
        if (collectFound == null)
        {
            return NotFound("Collect not found");
        }
        return Ok(collectFound);
    }

    [HttpPost(Name = "Create collect")]
    public IActionResult Save([FromBody] CollectRequestDto collect)
    {
        var collectToSave = collect.CollectRequestDtoToCollect();
        var collectSaved = _dbContext.Collects.Add(collectToSave);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(FindById), new {id = collectToSave.Id}, collectSaved);
    }
    
    [HttpPut("{id:int}", Name = "Update collect")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CollectRequestDto collect)
    {
        var collectToUpdate = await _dbContext.Collects.FindAsync(id);
        if (collectToUpdate == null)
        {
            return NotFound("Collect not found");
        }
        collectToUpdate.WasteType = collect.WasteType;
        await _dbContext.SaveChangesAsync();
        return Ok(collectToUpdate);
    }
    
    [HttpDelete("{id:int}", Name = "Delete collect")]
    public IActionResult Delete([FromRoute] int id)
    {
        var collectToDelete = _dbContext.Collects.Find(id);
        if (collectToDelete == null)
        {
            return NotFound("Collect not found");
        }
        _dbContext.Collects.Remove(collectToDelete);
        _dbContext.SaveChanges();
        return NoContent();
    }
    
}