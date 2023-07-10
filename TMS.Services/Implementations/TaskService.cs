using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TMS.Services.Implementations.TaskService;
using TMS.Data.Interfaces;
using TMS.Models.Entities;
using TMS.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TMS.Models.Dtos.Requests;

namespace TMS.Services.Implementations
{
    public class TaskService : ITaskService
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IServiceFactory _serviceFactory;
            private readonly IRepository<Tasks> _taskRepo;
            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;
            public TaskService(IServiceFactory serviceFactory, IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _serviceFactory = serviceFactory;
                _mapper = _serviceFactory.GetService<IMapper>();
                _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
                _taskRepo = _unitOfWork.GetRepository<Tasks>();
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
                Tasks taskList =  await _taskRepo.GetSingleByAsync(x => x.Id == Id);

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
                bool taskExists = await _taskRepo.AnyAsync(c =>
                    c.IsComplete && c.Description.ToLower() == request.Description.ToLower());

                if (taskExists)
                    throw new InvalidOperationException("Item already exists");

                Tasks newToDoItem = _mapper.Map<Tasks>(request);

                await _taskRepo.AddAsync(newToDoItem);
            }

            public async Task UpdateToDoItem(string Id, CreateToDoItemRequest request)
            {
                Tasks tasks = await _taskRepo.GetSingleByAsync(c => c.Id == Id);

                if (tasks == null)
                    throw new InvalidOperationException("Item doesnt exist");

                Tasks newToDoItem = _mapper.Map(request, tasks);

                await _taskRepo.UpdateAsync(newToDoItem);
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

            /*public async Task PatchToDoItem(int Id, JsonPatchDocument<CreateToDoItemRequest> request)
            {
                ToDoItem toDoItem = await GetToDoItem(Id);

                if (toDoItem == null)
                    throw new InvalidOperationException("No to do item found");

                CreateToDoItemRequest studentDataToUpdate = _mapper.Map<CreateToDoItemRequest>(toDoItem);

                request.ApplyTo(studentDataToUpdate);

                ToDoItem UpdatedToDoItem = _mapper.Map(studentDataToUpdate, toDoItem);

                await _taskRepo.UpdateAsync(UpdatedToDoItem);
            }*/
        }
    }
