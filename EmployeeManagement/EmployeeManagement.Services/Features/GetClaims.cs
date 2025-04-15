using System.Security.Claims;
using EmployeeManagement.Core.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Features
{
    public class GetClaims
    {
        public class GetClaimsQuery : IRequest<Unit>
        {
            public ICollection<Claim> Claims;
            public IServiceScope ServiceScope { get; set; }
            public GetClaimsQuery(ICollection<Claim> claims, IServiceScope serviceScope)
            {
                Claims = claims;
                ServiceScope = serviceScope;
            }
        }

        public class Handler : IRequestHandler<GetClaimsQuery, Unit>
        {
            private readonly ILogger<Handler> _logger;
            //private readonly IClaimsPrincipalProvider _claimsPrincipalProvider;
            //private readonly IServiceProvider _serviceProvider;

            public Handler(ILogger<Handler> logger, IClaimsPrincipalProvider claimsPrincipalProvider, IServiceProvider serviceProvider)
            {
                _logger = logger;
                //_claimsPrincipalProvider = claimsPrincipalProvider;
                //_serviceProvider = serviceProvider;
            }
            public Task<Unit> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("GetClaimsHandler called");
                //using var scope = _serviceProvider.CreateScope();
                var claimsProvider = request.ServiceScope.ServiceProvider.GetRequiredService<IClaimsPrincipalProvider>();
                var claimPrincipal = claimsProvider.Principal;

                var claims = claimPrincipal?.Claims;
                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        Console.WriteLine($"Type: {claim.Type} ::: Value: {claim.Value}");
                    }
                }
                return Task.FromResult(Unit.Value);
            }
        }
    }
}
