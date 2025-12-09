using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.Services.Interfaces;
using WebApiTrainingProject.Utils;

namespace WebApiTrainingProject.Controllers
{
    [Route("api/nodegraphs")]
    [ApiController]
    [Authorize]
    public class NodeGraphsController : ControllerBase
    {
        private readonly INodeGraphService _nodeGraphService;

        public NodeGraphsController(INodeGraphService nodeGraphService)
        {
            _nodeGraphService = nodeGraphService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var items = await _nodeGraphService.GetAllAsync(userId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _nodeGraphService.GetByIdAsync(id, userId);
            return Ok(result.JsonData);
        }

        [HttpPost]
        public async Task<ActionResult> Create(NodeGraphCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetUserId();
            var created = await _nodeGraphService.CreateAsync(dto, userId);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, NodeGraphUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetUserId();
            await _nodeGraphService.UpdateAsync(id, dto, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            await _nodeGraphService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
