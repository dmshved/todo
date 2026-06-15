using ToDo.Api.Dtos;
using ToDo.Api.Helpers;

namespace ToDo.Api.Services.Abstractions;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);

    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
}