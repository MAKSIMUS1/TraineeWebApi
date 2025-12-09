using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Services.Interfaces;
using WebApiTrainingProject.Utils;

namespace WebApiTrainingProject.Controllers
{
    [Route("api/projects")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            var userId = User.GetUserId();
            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetUserId();
            var project = await _projectService.CreateAsync(userId, dto);
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(Guid id, UpdateProjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetUserId();
            var updated = await _projectService.UpdateAsync(userId, id, dto);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _projectService.DeleteAsync(userId, id);

            return NoContent();
        }
    }
}
