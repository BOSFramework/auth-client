using BOS.Auth.Client.ClientModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BOS.Auth.Client.Responses
{
    public class UserResponse<T> : BOSWebServiceResponse where T : IUser
    {
        public T User { get; set; }

        public UserResponse(HttpStatusCode statusCode)
            : base(statusCode)
        {
        }
    }
}
