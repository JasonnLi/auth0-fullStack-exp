using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Utils
{
    // utilitie used for getting user info (claim) from user coming from the client side
    public static class UserUtils
    {
        // ClaimsPrincipal represents user, Identity represents user's passport, Claim represents statement on the password, sucha as name, datebirth, etc.
        public static bool GetUserId(ClaimsPrincipal user, out int userId)
        {
            var val = user.FindFirst(AuthClaims.UserId)?.Value;
            if (val != null)
            {
                userId = int.Parse(val);
                return true;
            }
            userId = -1;
            return false;
        }

        public static string GetAuth0UserId(ClaimsPrincipal user)
        {
            // ? here represents it could be null
            return user.FindFirst(AuthClaims.Auth0UserId)?.Value;
        }

        // out as a parameter modifier, which lets you pass an argument to a method by reference rather than by value
        public static bool GetTenantId(ClaimsPrincipal user, out Guid tenantId)
        {
            var idStr = user.FindFirst(AuthClaims.Auth0TenantId)?.Value;
            if (idStr == null)
            {
                tenantId = default(Guid);
                return false;
            }
            // verifying whether they are both in correct Guid format
            return Guid.TryParse(idStr, out tenantId);
        }
    }
}