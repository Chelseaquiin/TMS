using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Models.Dtos.Responses
{
    public class RoleResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ClaimCount { get; set; }
        public bool Active { get; set; }
    }
}
