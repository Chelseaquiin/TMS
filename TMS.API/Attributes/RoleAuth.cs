using Microsoft.AspNetCore.Authorization;

namespace TMS.API.Attributes
{
    public class AuthRequirement : IAuthorizationRequirement
    {
        private readonly string _routeName;
    }
}
