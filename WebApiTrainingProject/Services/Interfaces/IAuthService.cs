using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;

namespace WebApiTrainingProject.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterAsync(RegisterDto request);
        Task<TokenDto> LoginAsync(LoginDto request);
    }
}
