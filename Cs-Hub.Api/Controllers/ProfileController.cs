using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Security.Claims;

namespace Cs_Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public ProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var result = await _userProfileService.GetMyProfile(Guid.Parse(userId));
            if(!result.Succeed)
                return NotFound(result.Error);

            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(UpdateProfileDto updateProfileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userProfileService.UpdateMyProfile(Guid.Parse(userId), updateProfileDto);

            if (!result.Succeed)
                return BadRequest(result.Error);

            return Ok(result);
        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<PublicProfileDto>> GetPublicProfile(string username)
        {
            var result = await _userProfileService.GetPublicProfile(username);
            if (!result.Succeed)
                return NotFound(result.Error);

            return Ok(result);
        }
    }
}
