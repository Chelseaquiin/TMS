using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Requests;
using TMS.Models.Dtos.Responses;

namespace TMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<AccountResponse> CreateNewUser(AdminUserRegistrationRequest request);
        Task DeleteUser(string email);
        Task<AccountResponse> UpdateUser(string email, UpdateUserRequest request);
    }
}
