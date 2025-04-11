using System.Security.Claims;

namespace EmployeeManagement.Services.Contracts
{
    public interface IClaimsPrincipalProvider
    {
        ClaimsPrincipal Principal { get; set; }
    }
}
