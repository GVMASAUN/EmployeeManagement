using EmployeeManagement.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
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
                var taskWrapper = await _taskQueue.DequeueAsync(cancellationToken);

                _ = Task.Run(async () =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var claimsProvider = scope.ServiceProvider.GetRequiredService<IClaimsPrincipalProvider>();
                    claimsProvider.Principal = taskWrapper.ClaimsPrincipal;

                    //var mediator = scope.ServiceProvider.GetRequiredService<>();


                    await taskWrapper.WorkItem(scope, cancellationToken);
                }, cancellationToken);
            }
            _logger.LogInformation("Queued Processor Background Service is stopping.");
        }
    }
}
