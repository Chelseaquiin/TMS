using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Enums;

namespace TMS.Models.Entities
{
    public class Tasks
    {
        public Tasks()
        {
            IsComplete = false;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public bool IsComplete { get; set; }
        public Priority Priority { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 
    }
}
