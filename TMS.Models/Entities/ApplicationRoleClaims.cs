using Microsoft.AspNetCore.Identity;

namespace TMS.Models.Entities
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public bool Active { get; set; } = true;
    }
}
