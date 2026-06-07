using System.Threading.Tasks;
using GoogleClassroom.API.DTOs.Auth;

namespace GoogleClassroom.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(RegisterDto model);
        Task<TokenResponseDto> LoginAsync(LoginDto model);
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenDto model);
    }
}
