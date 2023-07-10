using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Requests;
using TMS.Models.Dtos.Responses;

namespace TMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Response<RoleResponse>> AddUserToRole(string userId, string roleName);
        Task<RoleResponse> CreateRole(RoleDto request);
        Task<Response<RoleResponse>> DeleteRole(string name);
        Task<Response<RoleResponse>> EditRole(string id, string name);
        Task<Response<IEnumerable<string>>> GetAllRoles();
    }
}
