using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Entities;
using TMS.Services.Interfaces;

namespace TMS.Services.Implementations
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly IRepository<ApplicationRoleClaim> _roleClaimRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;


        public RoleClaimService(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            _roleClaimRepo = _unitOfWork.GetRepository<ApplicationRoleClaim>();
        }

        public async Task<RoleClaimResponse> AddClaim(RoleClaimRequest request)
        {
            var getRole = await _unitOfWork.GetRepository<ApplicationRole>().GetSingleByAsync(r => r.Name.ToLower() == request.Role.ToLower()) ??
                throw new KeyNotFoundException("Role does not exist. Ensure there are no spaces in the text entered");

            var checkExisting = await _roleClaimRepo.GetSingleByAsync(x => x.ClaimType == request.ClaimType && x.RoleId == getRole.Id);
            if (checkExisting != null)
                throw new InvalidOperationException("Identical claim value already exist for this role");

            var newClaim = new ApplicationRoleClaim()
            {
                RoleId = getRole.Id,
                ClaimType = request.ClaimType,
            };

            await _roleClaimRepo.AddAsync(newClaim);

            return new RoleClaimResponse()
            {
                Role = getRole.Name,
                ClaimType = newClaim.ClaimType
            };

        }


        public async Task<IEnumerable<RoleClaimResponse>> GetUserClaims(string? role)
        {

            ApplicationRole getRole = await _unitOfWork.GetRepository<ApplicationRole>().GetSingleByAsync(x => x.Name.ToLower() == role.ToLower()) ??
                throw new KeyNotFoundException("Role does not exist. Ensure there are no spaces in the text entered");

            IEnumerable<ApplicationRoleClaim> claims = await _roleClaimRepo.GetByAsync(x => x.RoleId == getRole.Id);

            IList<RoleClaimResponse> roles = new List<RoleClaimResponse>();

            foreach (var claim in claims)
            {
                roles.Add(new RoleClaimResponse()
                {
                    Role = getRole.Name,
                    ClaimType = claim.ClaimType
                });
            }
            return roles;
        }

        public async Task RemoveUserClaims(string claimType, string role)
        {
            var getRole = await _unitOfWork.GetRepository<ApplicationRole>().GetSingleByAsync(x => x.Name.ToLower() == role.ToLower()) ??
                throw new KeyNotFoundException("Role does not exist. Ensure there are no spaces in the text entered");

            var claim = await _roleClaimRepo.GetSingleByAsync(x => x.ClaimType == claimType && x.RoleId == getRole.Id) ??
                throw new KeyNotFoundException("Claim value does not exist for this role");

            await _roleClaimRepo.DeleteAsync(claim);
        }
    }
}
