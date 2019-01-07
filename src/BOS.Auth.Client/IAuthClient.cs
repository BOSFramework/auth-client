using BOS.Auth.Client.ClientModels;
using BOS.Auth.Client.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BOS.Auth.Client
{
    public interface IAuthClient
    {
        // Actions related to users
        Task<UserResponse<T>> GetUserByEmailAsync<T>(string email) where T : IUser;
        Task<UserResponse<T>> GetUserByIdAsync<T>(Guid userId) where T : IUser;
        Task<GetUsersResponse<T>> GetUsersAsync<T>(bool filterDeleted = true) where T : IUser;
        Task<DeleteUserResponse> DeleteUserAsync(Guid userId);
        Task<UserResponse<T>> AddNewUserAsync<T>(string username, string email, string password) where T : IUser;
        Task<ExtendUserResponse> ExtendUserAsync(IUser user);
        Task<UpdateUserEmailResponse> UpdateUserEmailAsync(Guid userId, string email);
        Task<UpdateUsernameResponse> UpdateUsernameAsync(Guid userId, string newUsername);
        Task<GetUserRolesResponse<T>> GetUserRolesByUserIdAsync<T>(Guid userId) where T : IRole;
        Task<AddRoleToUserResponse> AddRoleToUserAsync(Guid roleId, Guid userId);
        Task<RevokeRoleResponse> RevokeRoleAsync(Guid roleId, Guid userId);

        // Actions related to roles
        Task<GetRolesResponse<T>> GetRolesAsync<T>(bool filterDeleted = true) where T : IRole;
        Task<AddRoleResponse<T>> AddRoleAsync<T>(string roleName) where T : IRole;
        Task<GetRoleByIdResponse<T>> GetRoleByIdAsync<T>(Guid roleId) where T: IRole;
        Task<DeleteRoleResponse> DeleteRoleAsync(Guid roleId);

        // Actions related to credentials and verifications
        Task<LoginResponse> SignInAsync(string username, string password);
        Task<PasswordChangeResponse> ForcePasswordChangeAsync(Guid userId, string newPassword);
        Task<PasswordChangeResponse> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

        // Actions related to slugs
        Task<CreateSlugResponse> CreateSlugAsync(string email);
        Task<VerifySlugResponse> VerifySlugAsync(string slug);
        Task<DeleteSlugResponse> DeleteSlugAsync(string slug);

    }
}
