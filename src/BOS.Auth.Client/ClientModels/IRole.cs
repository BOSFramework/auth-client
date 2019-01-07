using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.Auth.Client.ClientModels
{
    public interface IRole
    {
        Guid Id { get; set; }
        string Name { get; set; }
        bool Deleted { get; set; }
    }
}
