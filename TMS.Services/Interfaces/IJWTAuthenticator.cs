using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Responses;
using TMS.Models.Entities;

namespace TMS.Services.Interfaces
{
    public interface IJWTAuthenticator
    {
        Task<JwtToken> GenerateJwtToken(ApplicationUser user, string expires = null, List<Claim> additionalClaims = null);
        Task<JwtToken> GenerateMagicLinkToken(VoterProfile user, DateTime expires);
    }
}
