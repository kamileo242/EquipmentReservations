using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EquipmentReservations.DataLayer
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUser()
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.FindFirstValue(ClaimTypes.Name);
        }

        public IEnumerable<string> GetUserRoles()
        {
            var user = httpContextAccessor.HttpContext?.User;

            return user?
                .Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                ?? Enumerable.Empty<string>();
        }
    }
}
