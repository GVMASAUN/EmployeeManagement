using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Core.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Core.Services
{
    public class BackgroundClaimProcessor : IBackgroundClaimProcessor
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IClaimsPrincipalProvider _claimsPrincipalProvider;

        public BackgroundClaimProcessor(IBackgroundTaskQueue taskQueue, IClaimsPrincipalProvider claimsPrincipalProvider)
        {
            _taskQueue = taskQueue;
            _claimsPrincipalProvider = claimsPrincipalProvider;
        }

        public void AddClaimAndEnqueue(Func<IServiceScope, CancellationToken, Task> workItem)
        {
            var claimsPrincipal = _claimsPrincipalProvider.Principal;

            if (claimsPrincipal != null)
            {
                var claimQueueWorkItem = new BackgroundTaskWrapper(
                    claimsPrincipal,
                    workItem);

                _taskQueue.QueueBackgroundWorkItem(claimQueueWorkItem);
            }
        }
    }
}
