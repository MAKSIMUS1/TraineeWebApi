using AutoMapper;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Implementations;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId)
        {
            var items = await _projectRepository.GetAllByUserAsync(userId);
            return _mapper.Map<List<ProjectDto>>(items);
        }

        public async Task<ProjectDto> CreateAsync(Guid userId, CreateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            project.UserId = userId;

            await _projectRepository.AddAsync(project);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> UpdateAsync(Guid userId, Guid id, UpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null || project.UserId != userId)
                return null;

            _mapper.Map(dto, project);

            await _projectRepository.UpdateAsync(project);

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null || project.UserId != userId)
                return false;

            await _projectRepository.DeleteAsync(id);

            return true;
        }
    }

}
