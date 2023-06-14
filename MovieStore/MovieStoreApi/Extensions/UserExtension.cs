using System.Security.Claims;

namespace MovieStoreApi.Extensions
{
    public static class UserExtenisons
    {
        public static string GetUserEmail(this ClaimsPrincipal User)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            return email ?? throw new InvalidOperationException();
        }

        public static string GetUserName(this ClaimsPrincipal User)
        {
            var name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            return name ?? throw new InvalidOperationException();
        }
    }
}
