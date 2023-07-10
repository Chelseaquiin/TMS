using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Models.Dtos.Responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string error { get; set; }
    }

    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime Issued { get; set; }
        public DateTime? Expires { get; set; }
    }

    public class AuthenticationResponse
    {
        public JwtToken JwtToken { get; set; }
        public string UserType { get; set; }
        public string FullName { get; set; }
        public bool TwoFactor { get; set; }
        public string UserId { get; set; }

    }

    public class Response<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
