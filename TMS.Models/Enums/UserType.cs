using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Models.Enums
{
    public enum UserType
    {
        User = 1,
        Admin,
        SuperAdmin
    }

    public static class UserTypeExtension
    {
        public static string? GetStringValue(this UserType userType)
        {
            return userType switch
            {
                UserType.User => "user",
                UserType.Admin => "admin",
                UserType.SuperAdmin => "superadmin",
                _ => null
            };
        }
    }
}
