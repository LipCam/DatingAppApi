using System.Security.Claims;

namespace DatingAppApi.API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userName = user.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Cannot get username from token");
            return userName;
        }

        public static long GetUserId(this ClaimsPrincipal user)
        {
            var userId = long.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get id from token"));
            return userId;
        }
    }
}
