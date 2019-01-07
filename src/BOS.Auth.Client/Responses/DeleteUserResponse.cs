using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class DeleteUserResponse : BOSWebServiceResponse
    {
        public DeleteUserResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
