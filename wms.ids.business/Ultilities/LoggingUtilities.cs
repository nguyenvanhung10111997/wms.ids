using Microsoft.AspNetCore.Http;
using wms.ids.business;

namespace wms.ids.web.Utilities
{
    public static class LoggingUtilities
    {
        public static string WriteInfoLog(HttpContext context,string message)
        {
            var userIdentity = context.User;
            string username = userIdentity.Claims.Where(p => p.Type == CustomClaimType.Username).FirstOrDefault()?.Value;
            Console.WriteLine($"User {username} {message}");
            return string.Empty;
        }

        public static string WriteErrorLog(this HttpContext context,string message)
        {
            var userIdentity = context.User;
            string username = userIdentity?.Claims?.Where(p => p.Type == CustomClaimType.Username).FirstOrDefault()?.Value;
            Console.WriteLine($"User {username} {message}");

            return string.Empty;
        }
    }
}
