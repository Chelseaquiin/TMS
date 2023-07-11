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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("user-creation", Name = "user-creation")]
        [SwaggerOperation(Summary = "Admin creates user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "UserId of created user", Type = typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "User with provided email already exists", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create user", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateUser(AdminUserRegistrationRequest request)
        {
            var response = await _userService.CreateNewUser(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut("user-update", Name = "user-update")]
        [SwaggerOperation(Summary = "Updates user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "UserId of updated user", Type = typeof(AccountResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to update user", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]

        public async Task<IActionResult> UpdateUser(string email, [FromBody] UpdateUserRequest request)
        {
            var response = await _userService.UpdateUser(email, request);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpDelete("delete-user", Name = "delete-user")]
        [SwaggerOperation(Summary = "Deletes user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "User deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "User not found", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _userService.DeleteUser(email);
            return Ok();
        }

    }
}
