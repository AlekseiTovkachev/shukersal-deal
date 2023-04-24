using shukersal_backend.Models;
using System.Security.Claims;

namespace shukersal_backend.Utility
{
    public static class ServiceUtilities
    {
        public static Member? GetCurrentMember(MarketDbContext context, HttpContext httpContext)
        {
            string? username = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null)
            {
                return null;
            }
            return context.Members.Where(user => user.Username.Equals
            (username)).FirstOrDefault();
        }
    }
}
