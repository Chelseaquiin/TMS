using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Interfaces;
using TMS.Models.Commons;
using TMS.Models.Dtos.Requests;
using TMS.Models.Dtos.Responses;
using TMS.Models.Entities;
using TMS.Models.Enums;
using TMS.Services.Interfaces;

namespace TMS.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly IServiceFactory _serviceFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _userRepo;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJWTAuthenticator _jWTAuthenticator;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthenticationService(IServiceFactory serviceFactory, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IJWTAuthenticator jWTAuthenticator, IConfiguration configuration, IEmailService emailService)
        {
            /*   public AuthenticationService(IServiceFactory serviceFactory)
            {*/
            _serviceFactory = serviceFactory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            _mapper = _serviceFactory.GetService<IMapper>();
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _roleManager = _serviceFactory.GetService<RoleManager<ApplicationRole>>();
            _userRepo = _unitOfWork.GetRepository<ApplicationUser>();
            _jWTAuthenticator = jWTAuthenticator;
            _configuration = configuration;
            _emailService = emailService;
            _jWTAuthenticator = _serviceFactory.GetService<IJWTAuthenticator>();

        }

        public async Task<AccountResponse> CreateUser(UserRegistrationRequest request)
        {

            ApplicationUser existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                throw new InvalidOperationException($"User already exists with Email {request.Email}");

            existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
                throw new InvalidOperationException($"User already exists with username {request.UserName}");

            var roleName = Constants.DefaultRoleName;
            var userRole = await _roleManager.FindByNameAsync(roleName);

            ApplicationUser user = new()
            {

                Email = request.Email.ToLower(),
                UserName = request.UserName.Trim().ToLower(),
                FirstName = request.Firstname.Trim(),
                LastName = request.LastName.Trim(),
                PhoneNumber = request.MobileNumber,
                Active = true,
                UserTypeId = userRole.Type
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var message = $"Failed to create user: {(result.Errors.FirstOrDefault())?.Description}";
                throw new InvalidOperationException(message);

            }

            return new AccountResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Success = true,
                Message = "your account has been created"

            };

        }

        public async Task<AuthenticationResponse> UserLogin(LoginRequest request)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(request.UserName.ToLower().Trim());

            if (user == null)
                throw new InvalidOperationException("Invalid username or password");

            if (!user.Active)
                throw new InvalidOperationException("Account is not active");

            bool result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
                throw new InvalidOperationException("Invalid username or password");


            JwtToken userToken = await _jWTAuthenticator.GenerateJwtToken(user);


            string? userType = user.UserTypeId.GetStringValue();

            string fullName = string.IsNullOrWhiteSpace(user.MiddleName)
                ? $"{user.LastName} {user.FirstName}"
                : $"{user.LastName} {user.FirstName} {user.MiddleName}";


            return new AuthenticationResponse { JwtToken = userToken, UserType = userType, FullName = fullName, TwoFactor = false, UserId = user.Id };

        }


        public async Task<AccountResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new AccountResponse
                {
                    Success = false,
                    Message = "No user associated with email",
                };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string apUrl = $"{_configuration["AppUrl"]}/api/Auth/ResetPassword?email={email}&token={validToken}";
            string url = $"<p>Click <a href='{apUrl}'>here</a> to reset your password.</p>";

            var emailSent = await _emailService.SendResetPasswordEmailAsync(email, url);
            if (!emailSent)
                throw new InvalidOperationException("something went wrong");


            return new AccountResponse
            {
                Success = true,
                Message = "We've sent you a link via your email to reset your password",
            };

        }


        public async Task<AccountResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new InvalidDataException("No user associated with email");


            if (request.NewPassword != request.ConfirmPassword)
                throw new InvalidDataException("Password doesn't match its confirmation");


            var decodedToken = WebEncoders.Base64UrlDecode(request.AuthenticationToken);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, request.NewPassword);

            if (result.Succeeded)
                return new AccountResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Message = "Password has been reset successfully!",
                    Success = true,
                };

            throw new InvalidOperationException("Something went wrong");
        }



        private async Task<string> SaveChangedEmail(string userId, string decodedNewEmail, string decodedToken)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.ChangeEmailAsync(user, decodedNewEmail, decodedToken);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _unitOfWork.SaveChangesAsync();



            return "Email changed successfully";
        }

    }
}
