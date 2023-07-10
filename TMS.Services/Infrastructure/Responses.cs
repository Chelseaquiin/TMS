using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Services.Infrastructure
{
    public class Responses
    {
        public class SuccessResponse
        {
            public bool Success { get; set; }
            public object Data { get; set; }
        }

        public class ErrorResponse
        {
            public int Status { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
