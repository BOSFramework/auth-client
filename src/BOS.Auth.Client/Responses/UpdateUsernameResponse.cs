using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class UpdateUsernameResponse : BOSWebServiceResponse
    {
        public UpdateUsernameResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
