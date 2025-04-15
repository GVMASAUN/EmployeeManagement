using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Core.Contracts
{
    public interface IBackgroundClaimProcessor
    {
        public void AddClaimAndEnqueue(Func<IServiceScope, CancellationToken, Task> workItem);
    }
}
