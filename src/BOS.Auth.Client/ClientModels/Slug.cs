using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.Auth.Client.ClientModels
{
    public class Slug
    {
        public string Email { get; set; }
        public string Value { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool Deleted { get; set; }
    }
}
