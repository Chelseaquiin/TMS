using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Models.Enums;

namespace TMS.Models.Dtos.Requests
{
    public class CreateToDoItemRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
    }
}
