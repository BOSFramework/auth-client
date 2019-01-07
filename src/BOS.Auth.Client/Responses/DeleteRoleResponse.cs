using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class DeleteRoleResponse : BOSWebServiceResponse
    {
        public DeleteRoleResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
