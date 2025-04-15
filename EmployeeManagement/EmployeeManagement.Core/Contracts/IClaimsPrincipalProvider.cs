using System.Security.Claims;

namespace EmployeeManagement.Core.Contracts
{
    public interface IClaimsPrincipalProvider
    {
        ClaimsPrincipal Principal { get; set; }
    }
}
