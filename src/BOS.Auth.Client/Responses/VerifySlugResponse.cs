using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class VerifySlugResponse : BOSWebServiceResponse
    {
        public Guid UserId { get; set; }

        public VerifySlugResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
