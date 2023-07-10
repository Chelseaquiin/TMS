using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Models.Dtos.Requests
{
    public class EmailServiceRequest
    {
            public string Subject { get; set; }
            public string Message { get; set; }
            public List<string> EmailAddress { get; set; }
            public string ReceiverEmail { get; set; }
    }
}
