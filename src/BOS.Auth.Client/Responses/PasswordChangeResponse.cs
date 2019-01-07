using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class PasswordChangeResponse : BOSWebServiceResponse
    {
        public List<AuthError> Errors { get; set; }

        public PasswordChangeResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
            Errors = new List<AuthError>();
        }
    }
}
