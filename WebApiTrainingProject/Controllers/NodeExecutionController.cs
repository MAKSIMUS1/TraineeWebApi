using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiTrainingProject.Repositories.Interfaces;
using WebApiTrainingProject.Services.Implementations;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Controllers
{
    [Route("api/nodegraphs")]
    [ApiController]
    [Authorize]
    public class NodeExecutionController : ControllerBase
    {
        private readonly INodeGraphRepository _graphRepository;
        private readonly INodeExecutorService _executor;

        public NodeExecutionController(
            INodeGraphRepository graphRepository,
            INodeExecutorService executor)
        {
            _graphRepository = graphRepository;
            _executor = executor;
        }
        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpPost("{id}/execute")]
        public async Task<IActionResult> ExecuteGraph(Guid id)
        {
            var userId = GetUserId();

            var graph = await _graphRepository.GetByIdAsyncIncludeProject(id);
            if (graph == null)
                return NotFound("Graph not found");
            
            if (graph.Project.UserId != userId)
                return Forbid("You do not have access to this graph");
            
            if (string.IsNullOrWhiteSpace(graph.JsonData))
                return BadRequest("Graph JSON is empty");
            
            var result = await _executor.ExecuteGraphAsync(graph.JsonData);

            return Ok(new { Result = result });
        }
    }

}
