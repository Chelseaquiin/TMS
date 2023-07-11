using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Dtos.Responses;

namespace TMS.Services.Interfaces
{
    public interface IRoleClaimService
    {
        Task<RoleClaimResponse> AddClaim(RoleClaimRequest request);
        Task<IEnumerable<RoleClaimResponse>> GetUserClaims(string? role);
        Task RemoveUserClaims(string claimType, string role);
    }
}
