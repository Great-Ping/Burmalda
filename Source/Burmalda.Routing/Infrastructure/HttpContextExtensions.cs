using System.Security.Claims;

namespace Burmalda.Routing;

public static class HttpContextExtensions
{
    public static ulong GetUserId(this HttpContext context)
    {
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return ulong.Parse(userId);
    }
}