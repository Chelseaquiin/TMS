using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Models.Dtos.Responses
{
    public class RoleClaimResponse
    {
        public string Role { get; set; }
        public string ClaimType { get; set; }
    }

    public class RoleClaimRequest
    {
        public string Role { get; set; }
        public string ClaimType { get; set; }
    }
}
