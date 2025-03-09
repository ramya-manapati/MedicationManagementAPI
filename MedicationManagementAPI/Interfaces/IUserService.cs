using System.Security.Claims;

namespace MedicationManagementAPI.Interfaces
{
    public interface IUserService
    {
        int GetUserIdFromClaims(ClaimsPrincipal user);
    }
}


