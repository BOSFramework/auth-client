using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class RevokeRoleResponse : BOSWebServiceResponse
    {
        public RevokeRoleResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
