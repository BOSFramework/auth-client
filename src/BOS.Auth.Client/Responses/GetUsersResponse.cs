using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class GetUsersResponse<T> : BOSWebServiceResponse where T : IUser
    {
        public List<T> Users { get; set; }

        public GetUsersResponse(HttpStatusCode statusCode) 
            : base(statusCode)
        {
            Users = new List<T>();
        }
    }
}
