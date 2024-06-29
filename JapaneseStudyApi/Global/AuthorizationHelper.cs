using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JapaneseStudyApi.Global
{
    public static class AuthorizationHelper
    {
        public static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole("admin");
        }

        public static bool IsAuthorized(ClaimsPrincipal user, int userId)
        {
            var currentUser = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return currentUser != null && (IsAdmin(user) || currentUser == userId.ToString());
        }
    }
}