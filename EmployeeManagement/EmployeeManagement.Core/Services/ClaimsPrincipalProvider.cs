using System.Security.Claims;
using EmployeeManagement.Core.Contracts;

namespace EmployeeManagement.Core.Services
{
    public class ClaimsPrincipalProvider : IClaimsPrincipalProvider
    {
        public ClaimsPrincipal Principal { get; set; }
    }
}
