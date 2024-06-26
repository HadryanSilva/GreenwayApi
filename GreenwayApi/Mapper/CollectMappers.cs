using GreenwayApi.DTOs.Collect;
using GreenwayApi.Model;

namespace GreenwayApi.Mapper;

public static class CollectMappers
{
    public static CollectResponseDto CollectToResponseDto(this Collect collectModel)
    {
        return new CollectResponseDto()
        {
            Id = collectModel.Id,
            ScheduleDate = collectModel.ScheduleDate,
            UserId = collectModel.UserId,
            WasteType = collectModel.WasteType
        };
    }

    public static Collect CollectGetRequestDtoToCollect(this CollectGetRequestDto collectGetDto)
    {
        return new Collect()
        {
            ScheduleDate = collectGetDto.ScheduleDate,
            WasteType = collectGetDto.WasteType,
            UserId = collectGetDto.UserId
        };
    }
}