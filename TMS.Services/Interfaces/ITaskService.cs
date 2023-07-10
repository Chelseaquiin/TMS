using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Requests;
using TMS.Models.Entities;

namespace TMS.Services.Interfaces
{
    public interface ITaskService
    {
        Task CreateToDoItem(CreateToDoItemRequest request);
        Task DeleteToDoItem(string Id);
        Task<IEnumerable<Tasks>> GetAllToDoItems();
        Task<Tasks> GetToDoItem(string Id);
        Task ToggleToDoItem(string Id);
        Task UpdateToDoItem(string Id, CreateToDoItemRequest request);
    }
}
