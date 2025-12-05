using WebApiTrainingProject.Data.Models;

namespace WebApiTrainingProject.Repositories.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<List<Project>> GetAllByUserAsync(Guid userId);
    }
}
