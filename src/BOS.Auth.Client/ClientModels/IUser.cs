using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.Auth.Client.ClientModels
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        List<IRole> Roles { get; set; }
    }
}
