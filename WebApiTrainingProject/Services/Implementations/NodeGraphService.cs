using AutoMapper;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services.Implementations
{
    public class NodeGraphService : INodeGraphService
    {
        private readonly INodeGraphRepository _graphRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public NodeGraphService(
            INodeGraphRepository graphRepository,
            IProjectRepository projectRepository,
            IMapper mapper)
        {
            _graphRepository = graphRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<NodeGraphDto>> GetAllAsync(Guid userId)
        {
            var graphs = await _graphRepository.GetAllByUserIdAsync(userId);
            return _mapper.Map<List<NodeGraphDto>>(graphs);
        }

        public async Task<NodeGraphDto> GetByIdAsync(Guid id, Guid userId)
        {
            var graph = await _graphRepository.GetByIdAsyncIncludeProject(id);

            if (graph == null || graph.Project.UserId != userId)
                throw new Exception("Graph not found or access denied");

            return _mapper.Map<NodeGraphDto>(graph);
        }

        public async Task<NodeGraphDto> CreateAsync(NodeGraphCreateDto dto, Guid userId)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId);

            if (project == null || project.UserId != userId)
                throw new Exception("Project does not belong to user");

            var graph = _mapper.Map<NodeGraph>(dto);
            graph.Id = Guid.NewGuid();

            await _graphRepository.AddAsync(graph);

            return _mapper.Map<NodeGraphDto>(graph);
        }

        public async Task UpdateAsync(Guid id, NodeGraphUpdateDto dto, Guid userId)
        {
            var graph = await _graphRepository.GetByIdAsyncIncludeProject(id);

            if (graph == null || graph.Project.UserId != userId)
                throw new Exception("Graph not found or access denied");

            _mapper.Map(dto, graph);
            await _graphRepository.UpdateAsync(graph);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var graph = await _graphRepository.GetByIdAsyncIncludeProject(id);

            if (graph == null || graph.Project.UserId != userId)
                throw new Exception("Graph not found or access denied");

            await _graphRepository.DeleteAsync(graph.Id);
        }
    }
}
