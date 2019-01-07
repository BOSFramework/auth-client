using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BOS.Auth.Client.ClientModels;
using BOS.Auth.Client.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BOS.Auth.Client
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="apikey">BOS API key generated in BOS Console</param>
        public AuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Adds a new user with the given credentials.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserResponse<T>> AddNewUserAsync<T>(string username, string email, string password) where T : IUser
        {
            var payload = new { username, password, email };
            var response = await _httpClient.PostAsJsonAsync("Users?api-version=1.0", payload).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("Invalid Input");
            }

            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                var userResponse = new UserResponse<T>(response.StatusCode);
                userResponse.User = user;
                return userResponse;
            }

            var failedResponse = new UserResponse<T>(response.StatusCode);
            return failedResponse;
        }

        /// <summary>
        /// Add a role with the given name.
        /// </summary>
        /// <param name="roleName">Name of the role to be added.</param>
        /// <returns></returns>
        public async Task<AddRoleResponse<T>> AddRoleAsync<T>(string roleName) where T : IRole
        {
            var response = await _httpClient.PostAsync($"Roles?api-version=1.0&name={roleName}", null).ConfigureAwait(false);
            var addRoleResponse = new AddRoleResponse<T>(response.StatusCode);

            if (addRoleResponse.IsSuccessStatusCode)
            {
                addRoleResponse.Role = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }

            return addRoleResponse;
        }

        /// <summary>
        /// Add a role to a user.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AddRoleToUserResponse> AddRoleToUserAsync(Guid roleId, Guid userId)
        {
            var payload = new { roleId };
            var response = await _httpClient.PostAsJsonAsync($"Users({userId.ToString()})/AssignRole?api-version=1.0", payload).ConfigureAwait(false);
            var addRoleResponse = new AddRoleToUserResponse(response.StatusCode);

            if (addRoleResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                addRoleResponse.Errors.Add(new AuthError(101, "Must provide a valid role and user id"));
            }

            return addRoleResponse;
        }

        /// <summary>
        /// Pass the current password and new password for a user. If the current password matches the password on record then
        /// the password will be updated to the new value.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<PasswordChangeResponse> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var payload = new { currentPassword, newPassword };
            var response = await _httpClient.PostAsJsonAsync($"Users({userId.ToString()})/ChangePassword?api-version=1.0", payload).ConfigureAwait(false);
            var passwordChangeResponse = new PasswordChangeResponse(response.StatusCode);

            if (passwordChangeResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                passwordChangeResponse.Errors.Add(new AuthError(111, "The current password provided did not match the password on record"));
            }

            return passwordChangeResponse;
        }

        /// <summary>
        /// Creates an encrypted guid slug to be used in forgot password reset links.
        /// </summary>
        /// <param name="email">Email of the user who will have their password reset</param>
        /// <returns></returns>
        public async Task<CreateSlugResponse> CreateSlugAsync(string email)
        {
            var payload = new { email };
            var response = await _httpClient.PostAsJsonAsync("Slugs?api-version=1.0", payload).ConfigureAwait(false);
            var createSlugResponse = new CreateSlugResponse(response.StatusCode);

            if (createSlugResponse.IsSuccessStatusCode)
            {
                var slug = JsonConvert.DeserializeObject<Slug>(response.Content.ReadAsStringAsync().Result);
                createSlugResponse.Slug = slug;
            }

            return createSlugResponse;
        }

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<DeleteRoleResponse> DeleteRoleAsync(Guid roleId)
        {
            var response = await _httpClient.DeleteAsync($"Roles({roleId.ToString()})?api-version=1.0").ConfigureAwait(false);
            var deleteRoleResponse = new DeleteRoleResponse(response.StatusCode);
            return deleteRoleResponse;
        }

        /// <summary>
        /// Deletes a slug. Allows for a slug to be made unusable before its expiration date has passed.
        /// </summary>
        /// <param name="slug">The slug value to delete</param>
        /// <returns></returns>
        public async Task<DeleteSlugResponse> DeleteSlugAsync(string slug)
        {
            var response = await _httpClient.DeleteAsync($"Slugs('{slug}')?api-version=1.0").ConfigureAwait(false);
            var deleteSlugResponse = new DeleteSlugResponse(response.StatusCode);

            return deleteSlugResponse;
        }

        /// <summary>
        /// Delete the user with the provided Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<DeleteUserResponse> DeleteUserAsync(Guid userId)
        {
            var response = await _httpClient.DeleteAsync($"Users({userId.ToString()})").ConfigureAwait(false);

            var deleteResponse = new DeleteUserResponse(response.StatusCode);
            return deleteResponse;
        }

        public async Task<ExtendUserResponse> ExtendUserAsync(IUser user)
        {
            if (user.GetType().GetProperty("Password", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
            {
                throw new InvalidOperationException("Cannot update the users password using ExtendUser");
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_httpClient.BaseAddress}Users({user.Id.ToString()})?api-version=1.0");
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var updateUserEmailResponse = new ExtendUserResponse(response.StatusCode);

            return updateUserEmailResponse;
        }

        /// <summary>
        /// Override the password for the provided user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<PasswordChangeResponse> ForcePasswordChangeAsync(Guid userId, string newPassword)
        {
            var payload = new { newPassword };
            var response = await _httpClient.PostAsJsonAsync($"Users({userId.ToString()})/ForcePasswordChange?api-version=1.0", payload).ConfigureAwait(false);

            var pwResponse = new PasswordChangeResponse(response.StatusCode);
            return pwResponse;
        }

        /// <summary>
        /// Retrieves the role for the given Id.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<GetRoleByIdResponse<T>> GetRoleByIdAsync<T>(Guid roleId) where T : IRole
        {
            var response = await _httpClient.GetAsync($"Roles({roleId.ToString()})?api-version=1.0").ConfigureAwait(false);
            var getRoleByIdResponse = new GetRoleByIdResponse<T>(response.StatusCode);

            getRoleByIdResponse.Role = getRoleByIdResponse.IsSuccessStatusCode ?
                JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result) : default(T);

            return getRoleByIdResponse;
        }

        /// <summary>
        /// Get all of the created roles.
        /// </summary>
        /// <param name="filterDeleted">Whether or not to filter roles marked as deleted.
        /// Defaults to true.</param>
        /// <returns></returns>
        public async Task<GetRolesResponse<T>> GetRolesAsync<T>(bool filterDeleted = true) where T : IRole
        {
            var response = new HttpResponseMessage();
            var getRolesResponse = new GetRolesResponse<T>(System.Net.HttpStatusCode.EarlyHints);

            if (filterDeleted)
            {
                response = await _httpClient.GetAsync($"Roles?$filter=Deleted eq false&api-version=1.0").ConfigureAwait(false);
            }
            else
            {
                response = await _httpClient.GetAsync($"Roles?api-version=1.0").ConfigureAwait(false);
            }

            getRolesResponse.StatusCode = response.StatusCode;

            if (getRolesResponse.IsSuccessStatusCode)
            {
                var rolesJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                var roles = JsonConvert.DeserializeObject<List<T>>(rolesJson["value"].ToString());
                getRolesResponse.Roles = roles;
            }

            return getRolesResponse;
        }

        /// <summary>
        /// Finds a user given their email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserResponse<T>> GetUserByEmailAsync<T>(string email) where T : IUser
        {
            var response = await _httpClient.GetAsync($"Users?$filter=Email eq '{email.ToString()}'&api-version=1.0").ConfigureAwait(false);
            var userResponse = new UserResponse<T>(response.StatusCode);

            if (!userResponse.IsSuccessStatusCode)
            {
                return userResponse;
            }
            var usersJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var users = JsonConvert.DeserializeObject<List<T>>(usersJson["value"].ToString());

            if (users.Count == 0)
            {
                userResponse.User = default(T);
                return userResponse;
            }

            userResponse.User = users[0];
            return userResponse;
        }

        /// <summary>
        /// Retrieves a user with the given Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserResponse<T>> GetUserByIdAsync<T>(Guid userId) where T : IUser
        {
            var response = await _httpClient.GetAsync($"Users({userId.ToString()})?api-version=1.0&").ConfigureAwait(false);
            var userResponse = new UserResponse<T>(response.StatusCode);

            if (!userResponse.IsSuccessStatusCode)
            {
                return userResponse;
            }

            userResponse.User = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            return userResponse;
        }

        /// <summary>
        /// Retrieves all of the roles for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetUserRolesResponse<T>> GetUserRolesByUserIdAsync<T>(Guid userId) where T : IRole
        {
            var response = await _httpClient.GetAsync($"UserRoles({userId.ToString()})?api-version=1.0").ConfigureAwait(false);
            var getUserRolesResponse = new GetUserRolesResponse<T>(response.StatusCode);

            if (!getUserRolesResponse.IsSuccessStatusCode)
            {
                return getUserRolesResponse;
            }

            var rolesJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            getUserRolesResponse.Roles = JsonConvert.DeserializeObject<List<T>>(rolesJson["value"].ToString());

            return getUserRolesResponse;
        }

        /// <summary>
        /// Gets all of the users.
        /// </summary>
        /// <param name="filterDeleted">Whether or not to filter out users who are marked as deleted.
        ///  Defaults to true.</param>
        /// <returns></returns>
        public async Task<GetUsersResponse<T>> GetUsersAsync<T>(bool filterDeleted = true) where T : IUser
        {
            var response = new HttpResponseMessage();
            var getUsersResponse = new GetUsersResponse<T>(System.Net.HttpStatusCode.EarlyHints);

            if (filterDeleted)
            {
                response = await _httpClient.GetAsync("Users?$filter=Deleted eq false&api-version=1.0").ConfigureAwait(false);
            }
            else
            {
                response = await _httpClient.GetAsync("Users?api-version=1.0");
            }

            getUsersResponse.StatusCode = response.StatusCode;

            if (!getUsersResponse.IsSuccessStatusCode)
            {
                return getUsersResponse;
            }

            var usersJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var users = JsonConvert.DeserializeObject<List<T>>(usersJson["value"].ToString());
            getUsersResponse.Users = users;
            
            return getUsersResponse;
        }

        /// <summary>
        /// Removes a role from the user with the provided Id.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RevokeRoleResponse> RevokeRoleAsync(Guid roleId, Guid userId)
        {
            var payload = new { roleId };
            var response = await _httpClient.PostAsJsonAsync($"Users({userId.ToString()})/RevokeRole", payload).ConfigureAwait(false);
            var revokeRoleResponse = new RevokeRoleResponse(response.StatusCode);

            return revokeRoleResponse;
        }

        /// <summary>
        /// Verified the username and password combination
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginResponse> SignInAsync(string username, string password)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("Verification?api-version=1.0", payload).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content.ReadAsStringAsync().Result);
                loginResponse.StatusCode = response.StatusCode;
                return loginResponse;
            }

            var failedResponse = new LoginResponse(response.StatusCode);
            return failedResponse;
        }

        /// <summary>
        /// Updates the user's email address;
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UpdateUserEmailResponse> UpdateUserEmailAsync(Guid userId, string email)
        {
            var payload = new { email };
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_httpClient.BaseAddress}Users({userId.ToString()})?api-version=1.0");
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var updateUserEmailResponse = new UpdateUserEmailResponse(response.StatusCode);

            if (updateUserEmailResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                updateUserEmailResponse.Errors.Add(new AuthError(409, "That email belongs to another user."));
            }

            return updateUserEmailResponse;
        }

        /// <summary>
        /// Update the username for the user with the provided Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUsername"></param>
        /// <returns></returns>
        public async Task<UpdateUsernameResponse> UpdateUsernameAsync(Guid userId, string newUsername)
        {
            var payload = new { username = newUsername };
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_httpClient.BaseAddress}Users({userId.ToString()})?api-version=1.0");
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var updateUsernameResponse = new UpdateUsernameResponse(response.StatusCode);

            return updateUsernameResponse;

        }

        /// <summary>
        /// Verifys a slug is valid. If it is, then it returns the Id of the user it was generated for. 
        /// Otherwise it returns a 404 NotFound.
        /// </summary>
        /// <param name="slug">The slug that the user is passing to reset the password.</param>
        /// <returns></returns>
        public async Task<VerifySlugResponse> VerifySlugAsync(string slug)
        {
            var response = await _httpClient.GetAsync($"Slugs('{slug}')").ConfigureAwait(false);
            var verifySlugResponse = new VerifySlugResponse(response.StatusCode);

            if (verifySlugResponse.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                verifySlugResponse.UserId = new Guid(json["userId"].ToString());
            }

            return verifySlugResponse;
        }
    }
}
