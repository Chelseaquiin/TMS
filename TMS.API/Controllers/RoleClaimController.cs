using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TMS.Models.Dtos.Responses;
using TMS.Services.Interfaces;
using static TMS.Services.Infrastructure.Responses;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleClaimController : ControllerBase
    {
        private readonly IRoleClaimService _roleClaimsService;

        public RoleClaimController(IRoleClaimService roleClaimsService)
        {
            _roleClaimsService = roleClaimsService;
        }

        [HttpGet("get-claims", Name = "get-claims")]
        [SwaggerOperation(Summary = "returns claims of selected role")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns claim types and values", Type = typeof(RoleClaimResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = " ", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetClaims(string role)
        {
            var result = await _roleClaimsService.GetUserClaims(role);
            return Ok(result);
        }

        [HttpPost("add-claim", Name = "add-claim")]
        [SwaggerOperation(Summary = "adds claim to role")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns claim type and value", Type = typeof(RoleClaimResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to add claim", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> AddClaim([FromBody] RoleClaimRequest request)
        {
            var result = await _roleClaimsService.AddClaim(request);
            return Ok(result);
        }

        [HttpPost("delete-claim", Name = "delete-claim")]
        [SwaggerOperation(Summary = "deletes claims")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to delete claim", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteClaim(string claimValue, string role)
        {
            await _roleClaimsService.RemoveUserClaims(claimValue, role);
            return Ok();
        }
    }
}

