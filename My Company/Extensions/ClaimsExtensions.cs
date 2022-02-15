//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Security.Claims;

namespace My_Company.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetId(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static string GetEmail(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(ClaimTypes.Email).Value;
        }
    }
}
