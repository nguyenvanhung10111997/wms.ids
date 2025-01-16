using Microsoft.AspNetCore.Http;

namespace wms.ids.web.Utilities
{
    public static class CookieUtilities
    {
        public static void SetCookie(this HttpResponse Response, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
    }
}
