using AutoMapper;
using Serilog;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetUserProjectsAsync(Guid userId)
        {
            try
            {
                var items = await _uow.Projects.GetAllByUserAsync(userId);
                return _mapper.Map<List<ProjectDto>>(items);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get all projects for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ProjectDto> CreateAsync(Guid userId, CreateProjectDto dto)
        {
            try
            {
                var project = _mapper.Map<Project>(dto);
                project.Id = Guid.NewGuid();
                project.UserId = userId;

                var result = await _uow.Projects.AddAsync(project);

                if (result == null || result.Id == Guid.Empty)
                {
                    Log.Error("Failed to create project for user {UserId}", userId);
                    throw new Exception("Failed to create project");
                }

                await _uow.SaveChangesAsync();

                Log.Information("Created Project {ProjectId} for user {UserId}", project.Id, userId);

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating project for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ProjectDto?> UpdateAsync(Guid userId, Guid id, UpdateProjectDto dto)
        {
            try
            {
                var project = await _uow.Projects.GetByIdAsync(id);

                if (project == null || project.UserId != userId)
                {
                    Log.Warning("Project {ProjectId} not found or access denied for user {UserId}", id, userId);
                    return null;
                }

                _mapper.Map(dto, project);

                var updated = await _uow.Projects.UpdateAsync(project);
                if (updated == null)
                {
                    Log.Error("Failed to update project {ProjectId}", id);
                    throw new Exception("Update failed");
                }

                await _uow.SaveChangesAsync();

                Log.Information("Updated Project {ProjectId} for user {UserId}", project.Id, userId);

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating project {ProjectId} for user {UserId}", id, userId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            try
            {
                var project = await _uow.Projects.GetByIdAsync(id);

                if (project == null || project.UserId != userId)
                {
                    Log.Warning("Project {ProjectId} not found or access denied for user {UserId}", id, userId);
                    return false;
                }

                var deleted = await _uow.Projects.DeleteAsync(id);
                if (!deleted)
                {
                    Log.Error("Failed to delete project {ProjectId}", id);
                    throw new Exception("Delete failed");
                }

                await _uow.SaveChangesAsync();

                Log.Information("Deleted Project {ProjectId} for user {UserId}", id, userId);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting project {ProjectId} for user {UserId}", id, userId);
                throw;
            }
        }
    }
}
