using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class GetRolesResponse<T> : BOSWebServiceResponse where T : IRole
    {
        public List<T> Roles { get; set; }

        public GetRolesResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
            Roles = new List<T>();
        }
    }
}
