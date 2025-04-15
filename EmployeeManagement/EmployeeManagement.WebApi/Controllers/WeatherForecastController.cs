using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Core.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EmployeeManagement.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        public readonly IHubContext<RealTimeHub> _hubContext;
        private readonly INotificationService _notificationService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubContext<RealTimeHub> hubContext, INotificationService notificationService)

        {
            _logger = logger;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("SendNotification")]
        public void SendNotification()
        {
            _notificationService.SendNotificationToAllAsync("Hello from WeatherForecastController");
        }

        [HttpPost("send-to-user/{userId}")]
        public async Task<IActionResult> SendNotificationToUser([FromQuery] string userId, [FromBody] string message)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(message))
            {
                _logger.LogWarning("UserId or message is null or empty.");
                return BadRequest("Group name and message are required.");
            }

            await _notificationService.SendNotificationToUserAsync(userId, message);
            _logger.LogInformation("Notification sent to user {UserId}: {Message}", userId, message);
            return Ok(new { Message = "Notification sent successfully." });
        }
    }
}
