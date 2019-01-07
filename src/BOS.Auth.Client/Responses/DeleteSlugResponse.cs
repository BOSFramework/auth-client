using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class DeleteSlugResponse : BOSWebServiceResponse
    {
        public DeleteSlugResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
