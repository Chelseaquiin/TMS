using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TMS.Services.Infrastructure.Responses;
using Swashbuckle.AspNetCore.Annotations;
using TMS.Models.Dtos.Requests;
using TMS.Models.Entities;
using TMS.Services.Interfaces;
using TMS.Models.Dtos.Responses;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskManager;
        public TasksController(ITaskService taskManager)
        {
            _taskManager = taskManager;
        }


        [HttpGet("get-all-to-do-items")]
        [SwaggerOperation(Summary = "Gets all items in the to do list")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Gets all items in the to do list", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetAllToDoItems()
        {
            IEnumerable<Tasks> response = await _taskManager.GetAllToDoItems();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("get-to-do-item")]
        [SwaggerOperation(Summary = "Gets item with id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Gets item with id", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetToDoItem(string Id)
        {
            Tasks response = await _taskManager.GetToDoItem(Id);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("create-to-do-item")]
        [SwaggerOperation(Summary = "Creates a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Creates a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> CreateToDoItem(CreateToDoItemRequest request)
        {
            await _taskManager.CreateToDoItem(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("update-to-do-item")]
        [SwaggerOperation(Summary = "Update a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Updates a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> UpdateToDoItem(string Id, CreateToDoItemRequest request)
        {
            await _taskManager.UpdateToDoItem(Id, request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("delete-to-do-item")]
        [SwaggerOperation(Summary = "Delete a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Deletes a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> DeleteToDoItem(string Id)
        {
            await _taskManager.DeleteToDoItem(Id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("toggle-to-do-item")]
        [SwaggerOperation(Summary = "toggles a to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "toggles a to do item", Type = typeof(SuccessResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid username or password", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Description = "It's not you, it's us", Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ToDoItemResponse>> ToggleToDoItem(string Id)
        {
            await _taskManager.ToggleToDoItem(Id);
            return Ok();
        }

        /*[AllowAnonymous]
        [HttpPatch("patch-to-do-list")]
        [SwaggerOperation(Summary = "Patches to do item")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Profile patched successfully", Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "No profile found for user", Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Failed to update profile", Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateStudentProfile(int Id, JsonPatchDocument<CreateToDoItemRequest> request)
        {
            await _taskManager.PatchToDoItem(Id, request);
            return Ok();
        }*/

    }
}

