using WebApiTrainingProject.Data.Models;

namespace WebApiTrainingProject.Repositories.Interfaces
{
    public interface INodeGraphRepository : IBaseRepository<NodeGraph>
    {
        Task<NodeGraph> GetByIdAsyncIncludeProject(Guid id);
        Task<List<NodeGraph>> GetAllByUserIdAsync(Guid userId);
    }
}
