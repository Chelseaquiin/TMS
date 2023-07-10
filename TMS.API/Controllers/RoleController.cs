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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpPost("Create-Role", Name = "Create-New-Role")]
        [SwaggerOperation(Summary = "Creates user")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "UserId of created user", Type = typeof(RoleResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Role already exists", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "UserType already exists", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to create role", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateRole(RoleDto request)
        {
            var response = await _roleService.CreateRole(request);
            return Ok(response);
        }

        [HttpPut("Edit-Role", Name = "Edit-Role")]
        [SwaggerOperation(Summary = "Edits An Existing Role")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Role name has been updated", Type = typeof(RoleResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "UserId does not exist", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to Edit Role", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult> EditRole(string id, string name)
        {
            var response = await _roleService.EditRole(id, name);
            return Ok(response);
        }

        [HttpDelete("Delete-Role", Name = "Delete-Existing-Role")]
        [SwaggerOperation(Summary = "Deletes An Existing Role")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Role Name that was created", Type = typeof(RoleResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "RoleName does not exist", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteRole(string name)
        {
            var response = await _roleService.DeleteRole(name);
            return Ok(response);
        }


        [HttpPut("Add-User-To-Role", Name = "Add-User-To-Role")]
        [SwaggerOperation(Summary = "Add User To Role")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Adds a User to an available role", Type = typeof(RoleResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "UserId does not exist", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "RoleName does not exist", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var result = await _roleService.AddUserToRole(userId, roleName);
            return Ok(result);

        }
        [HttpGet("Get-All-Roles", Name = "Get-All-Roles")]
        [SwaggerOperation(Summary = "Get All Roles")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "View all Available Roles", Type = typeof(RoleResponse))]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();
            return Ok(result);
        }

    }
}
