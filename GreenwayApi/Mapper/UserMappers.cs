using GreenwayApi.DTOs.User;
using GreenwayApi.Model;

namespace GreenwayApi.Mapper;

public static class UserMappers
{
    public static UserResponseDto UserToResponseDto(this User userModel)
    {
        return new UserResponseDto()
        {
            Id = userModel.Id,
            Username = userModel.Username,
            Email = userModel.Email,
            Role = userModel.Role,
            Collects = userModel.Collects
        };
    }

    public static User UserRequestDtoToUser(this UserRequestDto userDto)
    {
        return new User()
        {
            Username = userDto.Username,
            Password = userDto.Password,
            Email = userDto.Email,
            Role = userDto.Role,
        };
    }
}