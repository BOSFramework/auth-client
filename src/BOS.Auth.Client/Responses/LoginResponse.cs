using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class LoginResponse : BOSWebServiceResponse
    {
        public bool IsVerified { get; set; }
        public Guid? UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public LoginResponse(HttpStatusCode statusCode)
            : base(statusCode)
        {

        }
    }
}
