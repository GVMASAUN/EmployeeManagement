using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Core.Hubs
{
    public class RealTimeHub : Hub
    {
        private readonly ILogger _logger;

        public RealTimeHub(ILogger<RealTimeHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID not found in claims.");
                //await base.OnConnectedAsync();
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "You are now connected to the group.");

            _logger.LogInformation("User connected: {User}", userId);

            await base.OnConnectedAsync();
        }

        public override async  Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();

            var userId = httpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID not found in claims.");
                await base.OnDisconnectedAsync(exception);
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "You have been disconnected from the group.");

            await base.OnDisconnectedAsync(exception);

            _logger.LogInformation("User disconnected: {User}", userId);
        }
    }
}
