using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Enums;

namespace TMS.Models.Dtos.Requests
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Role Name cannot be empty"), MinLength(2), MaxLength(30)]
        public string Name { get; set; }
        public UserType UserType { get; set; }
    }
}
