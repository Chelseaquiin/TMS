using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Interfaces;
using TMS.Models.Entities;
using TMS.Services.Interfaces;

namespace TMS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<UserProfile> _userRepo;
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
            _userRepo = _unitOfWork.GetRepository<UserProfile>();
            _emailService = _serviceFactory.GetService<IEmailService>();
            _configuration = _serviceFactory.GetService<IConfiguration>();
            _jWTAuthenticator = _serviceFactory.GetService<IJWTAuthenticator>();
        }

        public async Task<AccountResponse> UpdateUser(string email, UpdateUserRequest request)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(request.Email) ?? throw new InvalidOperationException("User not found.");

            existingUser.FirstName = request.Firstname;
            existingUser.LastName = request.LastName;
            existingUser.Email = request.Email;
            existingUser.UserTypeId = request.UserTypeId;

            try
            {
                await _userManager.UpdateAsync(existingUser);
                await _userManager.AddToRoleAsync(existingUser, request.UserTypeId.ToString());
                return new AccountResponse()
                {
                    UserId = existingUser.Id,
                    UserName = existingUser.UserName,
                    Success = true,
                    Message = "Role updated successfully"
                };
            }
            catch (Exception)
            {
                return new AccountResponse()
                {
                    UserId = existingUser.Id,
                    UserName = existingUser.UserName,
                    Success = false,
                    Message = "Update failed"
                };
            }

        }

        public async Task<AccountResponse> CreateNewUser(AdminUserRegistrationRequest request)
        {

            var authenticate = new AuthenticationService(_serviceFactory, _unitOfWork, _userManager, _roleManager, _jWTAuthenticator, _configuration, _emailService);
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
