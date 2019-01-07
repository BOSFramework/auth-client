using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.Auth.Client.ClientModels
{
    public class AuthError
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public AuthError(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
