using WebApiTrainingProject.DTOs.Response;

namespace WebApiTrainingProject.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfileAsync(Guid userId);
    }

}
