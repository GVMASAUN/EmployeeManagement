using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Core.DTOs
{
    public struct BackgroundTaskWrapper
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public Func<IServiceScope, CancellationToken, Task> WorkItem { get; set; }

        public BackgroundTaskWrapper(ClaimsPrincipal claimsPrincipal, Func<IServiceScope, CancellationToken, Task> workItem)
        {
            ClaimsPrincipal = claimsPrincipal;
            WorkItem = workItem;
        }
    }
}
