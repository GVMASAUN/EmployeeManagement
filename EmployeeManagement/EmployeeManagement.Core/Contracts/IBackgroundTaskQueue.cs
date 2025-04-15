using EmployeeManagement.Core.DTOs;

namespace EmployeeManagement.Core.Contracts
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(BackgroundTaskWrapper backgroundTask);
        Task<BackgroundTaskWrapper> DequeueAsync(CancellationToken cancellationToken);
    }
}
