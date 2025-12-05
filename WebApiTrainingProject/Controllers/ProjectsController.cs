using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            var userId = GetUserId();
            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            var userId = GetUserId();
            var project = await _projectService.CreateAsync(userId, dto);
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(Guid id, UpdateProjectDto dto)
        {
            var userId = GetUserId();
            var updated = await _projectService.UpdateAsync(userId, id, dto);

            if (updated == null)
                return NotFound("Проект не найден или не принадлежит пользователю");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            var ok = await _projectService.DeleteAsync(userId, id);

            if (!ok)
                return NotFound("Проект не найден или не принадлежит пользователю");

            return NoContent();
        }
    }

}
