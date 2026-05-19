using Application.Features.Auth.DTOs;

namespace Application.Features.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
