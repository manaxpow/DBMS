public static class CookieHelper
{
    public static void SetCookie(HttpResponse response, string key, string value, int? expireTime)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expireTime.HasValue ? DateTime.UtcNow.AddMinutes(expireTime.Value) : (DateTime?)null
        };
        response.Cookies.Append(key, value, cookieOptions);
    }

    public static string? GetCookie(HttpRequest request, string key)
    {
        return request.Cookies[key];
    }

    public static void DeleteCookie(HttpResponse response, string key)
    {
        response.Cookies.Delete(key);
    }
}
