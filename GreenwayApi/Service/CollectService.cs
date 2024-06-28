using GreenwayApi.Data;
using GreenwayApi.DTOs.Collect;
using GreenwayApi.DTOs.Request;
using GreenwayApi.Exceptions;
using GreenwayApi.Mapper;
using GreenwayApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GreenwayApi.Service;

public class CollectService : ICollectService
{
    private readonly ApplicationDbContext _dbContext;

    public CollectService(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<ICollection<CollectResponseDto>> FindAll(RequestParams parameters)
    {
        var offset = (parameters.PageNumber - 1) * parameters.PageSize;
        var collects = await _dbContext.Collects
            .OrderBy(i => i.Id)
            .Skip(offset)
            .Take(parameters.PageSize)
            .ToListAsync();
        return collects.Select(i => i.CollectToResponseDto()).ToList();
    }

    public async Task<ICollection<CollectResponseDto>> FindAllByUser(RequestParams parameters)
    {
        var offset = (parameters.PageNumber - 1) * parameters.PageSize;
        var collects = await _dbContext.Collects
            .Where(i => i.UserId == parameters.UserId)
            .OrderBy(i => i.Id)
            .Skip(offset)
            .Take(parameters.PageSize)
            .ToListAsync();
        return collects.Select(i => i.CollectToResponseDto()).ToList();
    }

    public CollectResponseDto FindById(int id, Dictionary<string, string> claims)
    {
        var collectFound = _dbContext.Collects.Find(id);

        if (collectFound == null)
        {
            throw new Exception("Collect not found");
        }

        ValidateUser(claims, collectFound);
        return collectFound.CollectToResponseDto();
    }

    public CollectResponseDto Save(CollectGetRequestDto collect)
    {
        var collectToSave = collect.CollectGetRequestDtoToCollect();
        _dbContext.Collects.Add(collectToSave);
        _dbContext.SaveChanges();
        return collectToSave.CollectToResponseDto();
    }

    public CollectResponseDto Update(CollectPutRequestDto collect, Dictionary<string, string> claims)
    {
        var collectFound = _dbContext.Collects.Find(collect.Id);

        if (collectFound == null)
        {
            throw new Exception("Collect not found");
        }

        ValidateUser(claims, collectFound);

        collectFound.WasteType = collect.WasteType;
        _dbContext.Collects.Update(collectFound);
        _dbContext.SaveChanges();
        return collectFound.CollectToResponseDto();
    }

    public void Delete(int id, Dictionary<string, string> claims)
    {
        var collectToDelete = _dbContext.Collects.Find(id);

        if (collectToDelete == null)
        {
            throw new Exception("Collect not found");
        }

        ValidateUser(claims, collectToDelete);

        _dbContext.Collects.Remove(collectToDelete);
        _dbContext.SaveChanges();
    }

    private void ValidateUser(Dictionary<string, string> claims, Collect collect)
    {
        var userMail = claims["email"];
        var user = _dbContext.Users.FirstOrDefault(i => i.Email == userMail);
        if (collect.UserId != user.Id)
        {
            throw new NotAuthorizedException("This collection does not belong to this user");
        }
    }
}