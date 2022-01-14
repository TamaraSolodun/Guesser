using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Shared_Guesser.Helpers
{
    public static class BearerHelper
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            try
            {
                var claim = user.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier);
                int.TryParse(claim.Value, out int res);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public static int? GetUserTypeId(this ClaimsPrincipal user)
        {
            try
            {
                var claim = user.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role);
                if (claim != null)
                {
                    int.TryParse(claim.Value, out int res);
                    return res;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public static string GetBearerFromRequest(this HttpRequest request)
        {
            try
            {
                return request.Headers.FirstOrDefault(i => i.Key.Equals("Authorization")).Value.ToString().Replace("Bearer ", "");
            }
            catch
            {
                throw;
            }
        }
    }
}
