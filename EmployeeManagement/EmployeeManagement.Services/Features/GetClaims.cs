using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Services.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Features
{
    public class GetClaims
    {
        public class GetClaimsQuery : IRequest<Unit>
        {
            public ICollection<Claim> Claims;
            public GetClaimsQuery(ICollection<Claim> claims)
            {
                Claims = claims;
            }
        }

        public class Handler : IRequestHandler<GetClaimsQuery, Unit>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _context;
            private readonly IClaimsPrincipalProvider _claimsPrincipalProvider;

            public Handler(ILogger<Handler> logger, IMapper mapper, IHttpContextAccessor context, IClaimsPrincipalProvider claimsPrincipalProvider)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
                _claimsPrincipalProvider = claimsPrincipalProvider;
            }
            public Task<Unit> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("GetClaimsHandler called");

                var user = _claimsPrincipalProvider.Principal;

                var claims = user.Claims;
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
