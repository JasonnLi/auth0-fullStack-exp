using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Utils
{
    public static class AuthClaims
    {
        // This nameidentifier could be used to get the current logged in user Id in ASP.NET Core
        public const string Auth0UserId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        // namespace used to be added in the user app.meta_data with tenantId, and inject into user access token by using Auth0 rule
        public const string Auth0TenantId = "https://schemas.authappexp.io/identity/claims/tenantid";

        // this claim has been added in the UserCheckMiddleware, recording the current logged in user's id
        public const string UserId = "userId";

    }
}