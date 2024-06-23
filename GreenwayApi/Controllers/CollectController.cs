using GreenwayApi.Data;
using GreenwayApi.DTOs.Collect;
using GreenwayApi.Mapper;
using GreenwayApi.Model;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpGet(Name = "Find all collects (Required Admin)")]
    public IActionResult FindAll()
    {
        var collect = _dbContext.Collects.ToList().Select(c => c.CollectToResponseDto());
        return Ok(collect);
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