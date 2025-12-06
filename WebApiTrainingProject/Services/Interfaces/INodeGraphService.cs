using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;

namespace WebApiTrainingProject.Services.Interfaces
{
    public interface INodeGraphService
    {
        Task<List<NodeGraphDto>> GetAllAsync(Guid userId);
        Task<NodeGraphDto> GetByIdAsync(Guid id, Guid userId);
        Task<NodeGraphDto> CreateAsync(NodeGraphCreateDto dto, Guid userId);
        Task UpdateAsync(Guid id, NodeGraphUpdateDto dto, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }

}
