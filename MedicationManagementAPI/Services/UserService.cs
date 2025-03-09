using MedicationManagementAPI.Services;
using System.Security.Claims;
using System.Linq;
using MedicationManagementAPI.Interfaces;


namespace MedicationManagementAPI.Services
{
    public class UserService : IUserService
    {
        public int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            // Look for the userId claim that was added during JWT creation
            var userIdClaim = user?.Claims?.FirstOrDefault(c => c.Type == "userId")?.Value;

            return userIdClaim != null ? int.Parse(userIdClaim) : 0;  // Parse and return the userId, or 0 if not found
        }
    }
}



