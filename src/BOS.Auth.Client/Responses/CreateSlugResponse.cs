using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class CreateSlugResponse : BOSWebServiceResponse
    {
        public Slug Slug { get; set; }

        public CreateSlugResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
