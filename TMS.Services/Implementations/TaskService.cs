
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TMS.Data.Interfaces;
using TMS.Models.Dtos.Requests;
using TMS.Models.Entities;
using TMS.Services.Interfaces;

namespace TMS.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;
        private readonly IRepository<Tasks> _taskRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskService(IServiceFactory serviceFactory, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _taskRepo = _unitOfWork.GetRepository<Tasks>();
            _httpContextAccessor = _serviceFactory.GetService<IHttpContextAccessor>();
        }

        public async Task<IEnumerable<Tasks>> GetAllToDoItems()
        {
            IEnumerable<Tasks> toDoItemList = await _taskRepo.GetAllAsync();

            if (!toDoItemList.Any())
                throw new InvalidOperationException("To do list is empty");


            return toDoItemList;
        }

        public async Task<Tasks> GetToDoItem(string Id)
        {
            Tasks taskList = await _taskRepo.GetSingleByAsync(x => x.Id == Id);

            if (taskList == null)
                throw new InvalidOperationException("no item with that Id");

            return new Tasks
            {
                Title = taskList.Title,
                Description = taskList.Description

            };
        }



        public async Task CreateToDoItem(CreateToDoItemRequest request)
        {
            var username = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByNameAsync(username);
            bool taskExists1 = await _taskRepo.AnyAsync(c =>
                c.IsComplete && c.Description.ToLower() == request.Description.ToLower());

            var taskExists = await _taskRepo.GetSingleByAsync(c => c.Title.ToLower() == request.Title);
            if (taskExists != null)
                throw new InvalidOperationException("Item already exists");

            Tasks newToDoItem = new Tasks
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                UserId = user.Id,
                Priority = request.Priority
            };

            await _taskRepo.AddAsync(newToDoItem);
        }

        public async Task UpdateToDoItem(string Id, CreateToDoItemRequest request)
        {
            Tasks tasks = await _taskRepo.GetSingleByAsync(c => c.Id == Id);

            if (tasks == null)
                throw new InvalidOperationException("Item doesnt exist");

            tasks.Title = request.Title;
            tasks.Description = request.Description;
            tasks.UpdatedAt = DateTime.Now;
            tasks.Priority = request.Priority;

            await _taskRepo.UpdateAsync(tasks);
        }

        public async Task DeleteToDoItem(string Id)
        {
            Tasks toDoItem = await _taskRepo.GetSingleByAsync(c =>
                c.Id == Id);

            if (toDoItem == null)
                throw new InvalidOperationException("Item doesnt exist");

            await _taskRepo.DeleteAsync(toDoItem);
        }

        public async Task ToggleToDoItem(string Id)
        {
            Tasks toDoItem = await _taskRepo.GetSingleByAsync(c =>
                c.Id == Id);

            if (toDoItem == null)
                throw new InvalidOperationException("Item doesnt exist");
            toDoItem.IsComplete = !toDoItem.IsComplete;
            await _taskRepo.UpdateAsync(toDoItem);
        }
        public async Task AssignTaskToUser(string userId, string taskId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new InvalidOperationException($"User '{userId}' does not Exist!");
            }
            Tasks task = await _taskRepo.GetByIdAsync(taskId);

            if (task is null)
            {
                throw new InvalidOperationException($"Task '{taskId}' does not Exist!");
            }

            task.UserId = user.Id;
            task.UpdatedAt = DateTime.Now;

            await _taskRepo.UpdateAsync(task);
        }
    }
}
