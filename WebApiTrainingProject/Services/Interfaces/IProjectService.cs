using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;

namespace WebApiTrainingProject.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId);
        Task<ProjectDto> CreateAsync(Guid userId, CreateProjectDto dto);
        Task<ProjectDto?> UpdateAsync(Guid userId, Guid id, UpdateProjectDto dto);
        Task<bool> DeleteAsync(Guid userId, Guid id);
    }

}
