using System;
using Microsoft.AspNetCore;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace BOS.Auth.Client.ServiceExtension
{
    public static class ServicesConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="apiKey">BOS API Key generated in the BOS Console</param>
        public static void AddBOSAuthClient(this IServiceCollection services, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new NullReferenceException("BOS API Key must not be null or empty.");
            }

            services.AddHttpClient<IAuthClient, AuthClient>(client =>
            {
                client.BaseAddress = new Uri("https://apis.dev.bosframework.com/auth/odata/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            });
        }
    }
}
