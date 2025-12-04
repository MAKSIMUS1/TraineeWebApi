using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Response;
using WebApiTrainingProject.Services.Interfaces;

namespace WebApiTrainingProject.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var profile = await _userService.GetProfileAsync(userId);

            return Ok(profile);
        }
    }
}
