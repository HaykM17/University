using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Authentication;

namespace Application.Services.Abstract;

public interface IAuthService
{
    Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
}