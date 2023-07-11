using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendResetPasswordEmailAsync(string email, string url);
    }
}
