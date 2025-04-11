using System.Security.Claims;
using EmployeeManagement.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.WebApi.Authorization.Handlers
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == requirement.Role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
