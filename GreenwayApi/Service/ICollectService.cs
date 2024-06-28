using GreenwayApi.DTOs.Collect;
using GreenwayApi.DTOs.Request;

namespace GreenwayApi.Service;

public interface ICollectService
{
    Task<ICollection<CollectResponseDto>> FindAll(RequestParams parameters);

    Task<ICollection<CollectResponseDto>> FindAllByUser(RequestParams parameters);

    CollectResponseDto FindById(int id, Dictionary<string, string> claims);

    CollectResponseDto Save(CollectGetRequestDto collect);

    CollectResponseDto Update(CollectPutRequestDto collect, Dictionary<string, string> claims);

    void Delete(int id, Dictionary<string, string> claims);
}
