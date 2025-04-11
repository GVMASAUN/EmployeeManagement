using System.Security.Claims;
using EmployeeManagement.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Services.Services
{
    public class ClaimsPrincipalProvider : IClaimsPrincipalProvider
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public ClaimsPrincipalProvider(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}
        //public ClaimsPrincipal Principal => new ClaimsPrincipal();
        public ClaimsPrincipal Principal { get; set; }

        //ClaimsPrincipal IClaimsPrincipalProvider.Principal { get => Principal; set => throw new NotImplementedException(); }
    }
}
