using AutoMapper;
using Serilog;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services
{
    public class NodeGraphService : INodeGraphService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public NodeGraphService(
            IUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<NodeGraphDto>> GetAllAsync(Guid userId)
        {
            try
            {
                var graphs = await _uow.NodeGraphs.GetAllByUserIdAsync(userId);
                return _mapper.Map<List<NodeGraphDto>>(graphs);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all node graphs for user {UserId}", userId);
                throw;
            }
        }

        public async Task<NodeGraphDto> GetByIdAsync(Guid id, Guid userId)
        {
            try
            {
                var graph = await _uow.NodeGraphs.GetByIdAsyncIncludeProject(id);

                if (graph == null || graph.Project.UserId != userId)
                {
                    Log.Warning("NodeGraph {GraphId} not found or no access for user {UserId}", id, userId);
                    throw new Exception("Graph not found or access denied");
                }

                return _mapper.Map<NodeGraphDto>(graph);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting NodeGraph {GraphId} for user {UserId}", id, userId);
                throw;
            }
        }

        public async Task<NodeGraphDto> CreateAsync(NodeGraphCreateDto dto, Guid userId)
        {
            try
            {
                var project = await _uow.Projects.GetByIdAsync(dto.ProjectId);

                if (project == null || project.UserId != userId)
                {
                    Log.Warning("Project {ProjectId} does not belong to user {UserId}", dto.ProjectId, userId);
                    throw new Exception("Project does not belong to user");
                }

                var graph = _mapper.Map<NodeGraph>(dto);
                graph.Id = Guid.NewGuid();

                var result = await _uow.NodeGraphs.AddAsync(graph);
                if (result == null || result.Id == Guid.Empty)
                {
                    Log.Error("Failed to add NodeGraph for project {ProjectId}", dto.ProjectId);
                    throw new Exception("Failed to create node graph");
                }

                await _uow.SaveChangesAsync();

                Log.Information("Created NodeGraph {GraphId} for user {UserId}", graph.Id, userId);

                return _mapper.Map<NodeGraphDto>(graph);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating NodeGraph for user {UserId}", userId);
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, NodeGraphUpdateDto dto, Guid userId)
        {
            try
            {
                var graph = await _uow.NodeGraphs.GetByIdAsyncIncludeProject(id);

                if (graph == null || graph.Project.UserId != userId)
                {
                    Log.Warning("NodeGraph {GraphId} not found or no access for user {UserId}", id, userId);
                    throw new Exception("Graph not found or access denied");
                }

                _mapper.Map(dto, graph);

                var updated = await _uow.NodeGraphs.UpdateAsync(graph);
                if (updated == null)
                {
                    Log.Error("Failed to update NodeGraph {GraphId}", id);
                    throw new Exception("Update failed");
                }

                await _uow.SaveChangesAsync();

                Log.Information("Updated NodeGraph {GraphId} for user {UserId}", id, userId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating NodeGraph {GraphId} for user {UserId}", id, userId);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                var graph = await _uow.NodeGraphs.GetByIdAsyncIncludeProject(id);

                if (graph == null || graph.Project.UserId != userId)
                {
                    Log.Warning("NodeGraph {GraphId} not found or no access for user {UserId}", id, userId);
                    throw new Exception("Graph not found or access denied");
                }

                var deleted = await _uow.NodeGraphs.DeleteAsync(id);
                if (!deleted)
                {
                    Log.Error("Failed to delete NodeGraph {GraphId}", id);
                    throw new Exception("Delete failed");
                }

                await _uow.SaveChangesAsync();

                Log.Information("Deleted NodeGraph {GraphId} for user {UserId}", id, userId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting NodeGraph {GraphId} for user {UserId}", id, userId);
                throw;
            }
        }
    }
}
