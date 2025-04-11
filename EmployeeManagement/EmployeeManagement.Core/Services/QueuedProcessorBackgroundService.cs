using EmployeeManagement.Core.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Core.Services
{
    public class QueuedProcessorBackgroundService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public QueuedProcessorBackgroundService(IBackgroundTaskQueue taskQueue, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _taskQueue = taskQueue;
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<QueuedProcessorBackgroundService>();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Processor Background Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(_serviceProvider, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
            _logger.LogInformation("Queued Processor Background Service is stopping.");
        }
    }
}
