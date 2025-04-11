using System.Security.Claims;
using System.Text.Json;
using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Services.Contracts;
using EmployeeManagement.Services.Features;
using EmployeeManagement.Services.Services;
using EmployeeManagement.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebApi.Controllers
{
    [ApiController]
    [Route(RouteKeys.Background)]
    public class BackgroundTaskController : Controller
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IClaimsPrincipalProvider _claimsPrincipalProvider;

        public BackgroundTaskController(IBackgroundTaskQueue taskQueue, ILogger<BackgroundTaskController> logger, IMediator mediator, IClaimsPrincipalProvider claimsPrincipalProvider)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _mediator = mediator;
            _claimsPrincipalProvider = claimsPrincipalProvider;
        }

        [Authorize(AuthenticationSchemes = "JwtScheme")]
        [HttpPost(RouteKeys.ProcessTask)]
        public async Task<IActionResult> ProcessTask()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                string taskName = await reader.ReadToEndAsync();

                _taskQueue.QueueBackgroundWorkItem(
                    async (serviceProvider, cancellationToken) =>
                    {
                        _logger.LogInformation("Background task started: {TaskName}", taskName);
                        //_logger.LogInformation($"{HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
                        _logger.LogInformation($"{JsonSerializer.Serialize(HttpContext.User.Claims.ToList().Count())}");
                        await Task.Delay(5000, cancellationToken);

                        _logger.LogInformation("Background task completed: {TaskName}", taskName);
                    }
                );
            }
            catch (Exception exception)
            {
                throw new Exception("Error processing task", exception);
            }

            return Accepted(new { Message = "Task is being processed in the background." });
        }

        [HttpGet(RouteKeys.GetClaims)]
        [Authorize(AuthenticationSchemes = "JwtScheme")]
        public IActionResult GetClaims(CancellationToken cancellationToken)
        {
            var claimsList = HttpContext.User.Claims.ToList();
            var claimsPrincipal = HttpContext.User;

            _taskQueue.QueueBackgroundWorkItem(
                async (serviceProvider, cancellationToken) =>
                {
                    using var scope = serviceProvider.CreateScope();

                    var claimsProvider = scope.ServiceProvider.GetRequiredService<IClaimsPrincipalProvider>();
                    claimsProvider.Principal = _claimsPrincipalProvider.Principal;
                    //claimsProvider.Principal = claimsPrincipal;
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(new GetClaims.GetClaimsQuery(claimsList), cancellationToken);
                }
            );
            return Ok(new { Message = "Your request will be processed in background." });
        }
    }
}
