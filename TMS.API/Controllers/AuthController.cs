using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TMS.Models.Dtos.Requests;
using TMS.Models.Dtos.Responses;
using TMS.Services.Interfaces;
using static TMS.Services.Infrastructure.Responses;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login", Name = "Login")]
        [SwaggerOperation(Summary = "Authenticates user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "returns user Id", Type = typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
        {
            AuthenticationResponse response = await _authService.UserLogin(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password", Name = "Forgot password")]
        [SwaggerOperation(Summary = "forgot password")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "forgot password", Type = typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            AccountResponse response = await _authService.ForgotPasswordAsync(email);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("reset-password", Name = "Reset Password")]
        [SwaggerOperation(Summary = "forgot password")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "returns password reset succssfully", Type = typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ResetPasswordAsync(request);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

    }
}
