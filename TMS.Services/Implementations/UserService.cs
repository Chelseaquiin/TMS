using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TMS.Data.Interfaces;
using TMS.Models.Dtos.Requests;
using TMS.Models.Dtos.Responses;
using TMS.Models.Entities;
using TMS.Services.Interfaces;

namespace TMS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJWTAuthenticator _jWTAuthenticator;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _roleManager = _serviceFactory.GetService<RoleManager<ApplicationRole>>();
            _emailService = _serviceFactory.GetService<IEmailService>();
            _configuration = _serviceFactory.GetService<IConfiguration>();
            _jWTAuthenticator = _serviceFactory.GetService<IJWTAuthenticator>();
        }

        public async Task<AccountResponse> UpdateUser(string email, UpdateUserRequest request)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException("User not found.");

            existingUser.FirstName = request.Firstname;
            existingUser.LastName = request.LastName;
            existingUser.Email = request.Email;

            await _userManager.UpdateAsync(existingUser);
            return new AccountResponse()
            {
                UserId = existingUser.Id,
                UserName = existingUser.UserName,
                Success = true,
                Message = "User Updated Successfully"
            };
        }

        public async Task<AccountResponse> CreateNewUser(AdminUserRegistrationRequest request)
        {

            var authenticate = new AuthenticationService(_serviceFactory);
            var creationResult = await authenticate.CreateUser(new UserRegistrationRequest()
            {
                Password = request.Password,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                UserName = request.Username,
                Firstname = request.Firstname,
                LastName = request.LastName,
            });

            return creationResult;
        }

        public async Task DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException("Email not found.");

            await _userManager.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
