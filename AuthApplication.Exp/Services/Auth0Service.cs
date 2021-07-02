using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Microsoft.Extensions.Logging;

namespace AuthApp.Application.Exp.Services
{
    public class Auth0Service
    {
        // connection represent DB connection in Auth0
        public class CreateUserOptions
        {
            public string TenantId { get; set; }
            public string Connection { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UpdateUserOptions
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private ManagementTokenFactory tokenFactory;
        private ILogger logger;

        public Auth0Service(ManagementTokenFactory tokenFactory, ILogger<Auth0Service> logger)
        {
            this.tokenFactory = tokenFactory;
            this.logger = logger;
        }

        public async Task<ManagementApiClient> GetClient()
        {
            // Create an instance of the ManagementApiClient class with the token and the API URL of your Auth0 instance
            return new ManagementApiClient(await tokenFactory.GetTokenAsync(logger), tokenFactory.Domain);
        }

        public async Task<Auth0.ManagementApi.Models.User> CreateUser(CreateUserOptions options)
        {
            var client = await GetClient();
            return await client.Users.CreateAsync(new Auth0.ManagementApi.Models.UserCreateRequest()
            {
                AppMetadata = new
                {
                    tenant_id = options.TenantId
                },
                Email = options.Email,
                FirstName = options.FirstName,
                LastName = options.LastName,
                Connection = options.Connection,
                Password = options.Password
            });
        }

        public async Task<Auth0.ManagementApi.Models.User> GetUser(string auth0UserId)
        {
            var client = await GetClient();
            return await client.Users.GetAsync(auth0UserId);
        }

        public async Task SetUserTenantId(string auth0UserId, Guid tenantId)
        {
            var client = await GetClient();
            await client.Users.UpdateAsync(auth0UserId, new Auth0.ManagementApi.Models.UserUpdateRequest()
            {
                AppMetadata = new
                {
                    tenant_id = tenantId
                },
            });
        }

        public async Task UpdateUser(string auth0UserId, UpdateUserOptions options)
        {
            var client = await GetClient();
            await client.Users.UpdateAsync(auth0UserId, new Auth0.ManagementApi.Models.UserUpdateRequest()
            {
                FirstName = options.FirstName,
                LastName = options.LastName
            });
        }

        public async Task DeleteUser(string auth0Id)
        {
            var client = await GetClient();
            await client.Users.DeleteAsync(auth0Id);
        }
    }
}