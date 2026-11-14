using Cs_Hub.Application.DTO;
using Cs_Hub.Application.DTO.Profile;
using Cs_Hub.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cs_Hub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await _authService.Register(registerDTO);
            if (!result.Succeed)
                return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmail(userId, token);
            if (!result.Succeed)
                return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _authService.Login(loginDTO);
            if (!result.Succeed)
                return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.ChangePassword(Guid.Parse(userId!), dto);

            if (!result.Succeed)
                return BadRequest(result.Data);

            return Ok(result.Data);
        }

        [HttpPut("deactivate")]
        [Authorize]
        public async Task<IActionResult> AccountDeactivation(DeactivateAccountDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var result = await _authService.DeactivateAccount(Guid.Parse(userId!), dto);
            if (!result.Succeed)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
