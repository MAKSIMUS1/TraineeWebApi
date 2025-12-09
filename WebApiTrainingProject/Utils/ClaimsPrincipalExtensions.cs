using System.Security.Claims;

namespace WebApiTrainingProject.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("UserId claim not found in JWT");

            return Guid.Parse(value);
        }
    }
}
