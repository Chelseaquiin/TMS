using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Responses;

namespace TMS.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AccountResponse> CreateUser(UserRegistrationRequest request);
        Task<AccountResponse> ForgotPasswordAsync(string email);
        Task<AccountResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<AuthenticationResponse> UserLogin(LoginRequest request);
    }
}
