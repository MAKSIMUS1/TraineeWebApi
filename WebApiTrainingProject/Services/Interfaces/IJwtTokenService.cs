using WebApiTrainingProject.Data.Models;

namespace WebApiTrainingProject.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
    }
}
