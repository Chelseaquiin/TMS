using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Requests;
using TMS.Services.Interfaces;

namespace TMS.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email, string url)
        {
            EmailServiceRequest emailService = new EmailServiceRequest
            {
                EmailAddress = new string[] { email }.ToList(),
                Message = $"Click this button to reset your password. {url}",
                Subject = "Reset Password"
            };

            SendEmailResponse response = await SendEmailHelperAsync(emailService);
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                default:
                    return false;
            }
        }

        private async Task<SendEmailResponse> SendEmailHelperAsync(EmailServiceRequest request)
        {
            var accessKey = _configuration["AWS:AccessKey"];
            var secretKey = _configuration["AWS:SecretKey"];
            var region = _configuration["AWS:Region"];
            string sourceEmail = _configuration["AWS:SourceEmail"];
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonSimpleEmailServiceConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };
            var client = new AmazonSimpleEmailServiceClient(credentials, config);

            var message = new SendEmailRequest
            {
                Source = sourceEmail,
                Destination = new Destination
                {
                    ToAddresses = request.EmailAddress
                },
                Message = new Message
                {
                    Subject = new Content(request.Subject),
                    Body = new Body
                    {
                        Html = new Content(request.Message)
                    }
                }
            };
            return await client.SendEmailAsync(message);
        }
    }
}

