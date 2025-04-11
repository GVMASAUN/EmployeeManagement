using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.WebApi.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}
