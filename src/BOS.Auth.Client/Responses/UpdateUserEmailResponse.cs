using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class UpdateUserEmailResponse : BOSWebServiceResponse
    {
        public List<AuthError> Errors { get; set; }

        public UpdateUserEmailResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
            Errors = new List<AuthError>();
        }
    }
}
