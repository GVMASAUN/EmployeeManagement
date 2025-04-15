using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Channels;
using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Core.DTOs;

namespace EmployeeManagement.Core.Services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<BackgroundTaskWrapper> _queue;

        public BackgroundTaskQueue()
        {
            _queue = Channel.CreateUnbounded<BackgroundTaskWrapper>(new UnboundedChannelOptions
            {
                SingleReader = false,
                SingleWriter = false
            });
        }

        public void QueueBackgroundWorkItem(BackgroundTaskWrapper taskWrapper)
        {
            if (taskWrapper.WorkItem == null) throw new ArgumentNullException(nameof(taskWrapper.WorkItem));

            _queue.Writer.TryWrite(taskWrapper);
        }

        public async Task<BackgroundTaskWrapper> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);
            return workItem;

        }
    }
}