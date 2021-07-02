using AuthApp.Application.Exp.Utils;
using AuthApp.Core.Exp.Entities;
using AuthApp.Infra.Exp.Data;
using AuthApp.Infra.Exp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Extensions
{
    // ClaimsPrincipal object is the result after parsing an access_token
    // ClaimsPrincipalExtensions is used to collect DB info by using user claim data from HTTP request
    public static class ClaimsPrincipalExtensions
    {
        public static async Task<Customer> GetCustomer(this ClaimsPrincipal user, AuthAppContext context)
        {
            if (!UserUtils.GetTenantId(user, out var tenantGuid))
            {
                return null;
            }
            var customerRepo = new CustomerRepository(context);
            return await customerRepo.GetCustomerByPublicId(tenantGuid);
        }

        public static bool GetUserId(this ClaimsPrincipal user, out int userId)
        {
            return UserUtils.GetUserId(user, out userId);
        }

        public static async Task<User> GetUser(this ClaimsPrincipal user, AuthAppContext context)
        {
            // @TODO Can probably embed the CustomerId into the Claims if fetched before
            var customer = await user.GetCustomer(context);
            if (customer == null)
            {
                return null;
            }

            if (!UserUtils.GetUserId(user, out var userId))
            {
                return null;
            }

            var userRepo = new UserRepository(context);
            var userRecord = await userRepo.GetUserById(customer.Id, userId);
            if (userRecord == null)
            {
                throw new Exception("User not found");
            }
            return userRecord;
        }
    }
}