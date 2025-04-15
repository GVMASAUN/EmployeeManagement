using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<RealTimeHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IHubContext<RealTimeHub> hubContext, ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendNotificationToAllAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
            _logger.LogInformation("Notification sent to all clients: {Message}", message);
        }

        public async Task SendNotificationToUserAsync(string userId, string message)
        {
            await _hubContext.Clients.Groups(userId).SendAsync("ReceiveNotification", message);
            _logger.LogInformation("Notification sent to user {UserId}: {Message}", userId, message);
        }
    }
}
