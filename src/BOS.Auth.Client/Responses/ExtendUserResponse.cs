using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class ExtendUserResponse : BOSWebServiceResponse
    {
        public List<AuthError> Errors { get; set; }

        public ExtendUserResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
