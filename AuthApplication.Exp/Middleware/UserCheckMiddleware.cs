using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AuthApp.Application.Exp.Interfaces;
using AuthApp.Application.Exp.Services;
using AuthApp.Application.Exp.Utils;
using AuthApp.Core.Exp.Entities;
using AuthApp.Infra.Exp.Data;
using AuthApp.Infra.Exp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthApp.Core.Exp.Repositories;

namespace AuthApp.Application.Exp.Middleware
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Auth0Service auth0Service;
        // Allowing your to categroize your different messages in your logging system
        private readonly ILogger<UserCheckMiddleware> logger;

        public UserCheckMiddleware(
            RequestDelegate next,
            Auth0Service auth0Service,
            ILogger<UserCheckMiddleware> logger)
        {
            this.next = next;
            this.auth0Service = auth0Service;
            this.logger = logger;
        }

        /* If an authenticated user connects that doesnt exist in the database (via SSO etc)
         * Pull the user profile from auth0 and create a user record
         */

        public async Task Invoke(HttpContext context, AuthAppContext dbContext)
        {
            // @TODO Handle appId from claim? add to access_token

            if (context.User != null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var auth0UserId = UserUtils.GetAuth0UserId(context.User);
                    var hasTenantId = UserUtils.GetTenantId(context.User, out var tenantId);

                    var userRepo = new UserRepository(dbContext);
                    var customerRepo = new CustomerRepository(dbContext);

                    User localUser = null;
                    if (hasTenantId)
                    {
                        logger.LogInformation("HasTenantId: " + tenantId);
                        var customer = await customerRepo.GetCustomerByPublicId(tenantId);
                        if (customer == null)
                        {
                            logger.LogInformation("Could not find customer by tenantId");
                            context.Response.StatusCode = 401; //Unauthorized
                            await context.Response.WriteAsync("Invalid user");
                            return;
                        }
                        logger.LogInformation("Found customer: " + customer.Id.ToString());
                        logger.LogInformation("auth0UserId: " + auth0UserId);
                        localUser = await userRepo.GetUserByAuth0Id(customer.Id, auth0UserId);
                    }

                    // If we dont have a user record, create one in the db
                    if (localUser == null)
                    {
                        var auth0User = await auth0Service.GetUser(auth0UserId);
                        foreach (var identity in auth0User.Identities)
                        {
                            var provider = identity.Provider;
                            var con = identity.Connection;
                            logger.LogInformation("Identity Prov: " + provider + ", Con: " + con);

                            // Get customer via connection, the connection name is defined in Auth0 and accompany with customer org Id, so it can be identified
                            var customer = await customerRepo.GetCustomerFromAuth0Connection(con);
                            if (customer == null)
                            {
                                // @TODO This is bad
                                context.Response.StatusCode = 401; //Unauthorized
                                await context.Response.WriteAsync("Invalid user");
                                return;
                            }

                            // Create User DB record
                            localUser = await userRepo.CreateUser(customer.Id, new CreateUserOptions()
                            {
                                Auth0Id = auth0UserId,
                                Email = auth0User.Email,
                                FirstName = auth0User.FirstName,
                                LastName = auth0User.LastName,
                                Source = UserSource.IdentityProvider,
                                Type = UserType.Standard
                            });

                            // Update Auth0 app_metadata with tenantId
                            await auth0Service.SetUserTenantId(auth0User.UserId, customer.PublicId);

                            // @TODO Call some "provision user" function that assigns licences etc

                            // @TODO Read groups and create them if not existing. Tag them as IdentityProvider sourced

                            break;
                        }
                    }

                    if (localUser == null)
                    {
                        context.Response.StatusCode = 401; //Unauthorized
                        await context.Response.WriteAsync("Invalid user");
                        return;
                    }

                    // Add the userId as a claim
                    var claimsIdentity = context.User.Identity as ClaimsIdentity;
                    claimsIdentity.AddClaim(new Claim(AuthClaims.UserId, localUser.Id.ToString()));
                }
            }

            await next(context);
        }
    }
}