using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class GetRoleByIdResponse<T> : BOSWebServiceResponse where T : IRole
    {
        public T Role { get; set; }

        public GetRoleByIdResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
        }
    }
}
