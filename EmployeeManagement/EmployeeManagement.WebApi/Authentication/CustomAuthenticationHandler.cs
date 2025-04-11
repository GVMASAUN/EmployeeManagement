using System.Security.Claims;
using System.Text.Encodings.Web;
using EmployeeManagement.Core.Contracts;
using EmployeeManagement.Services.Contracts;
using EmployeeManagement.Services.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.WebApi.Authentication
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<CustomAuthenticationHandler> _logger;
        private readonly IOptionsMonitor<AuthenticationSchemeOptions> _options;
        private readonly UrlEncoder _urlEncoder;
        private IClaimsPrincipalProvider _claimsPrincipalProvider;

        public CustomAuthenticationHandler(
            IRedisCacheService redisCacheService,
            ITokenService tokenService,
            ILoggerFactory loggerFactory,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            UrlEncoder urlEncoder,
            IClaimsPrincipalProvider claimsPrincipalProvider
        ) : base(options, loggerFactory, urlEncoder)
        {
            _redisCacheService = redisCacheService;
            _tokenService = tokenService;
            _logger = loggerFactory.CreateLogger<CustomAuthenticationHandler>();
            _options = options;
            _urlEncoder = urlEncoder;
            _claimsPrincipalProvider = claimsPrincipalProvider;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Unauthorized - No Token Provided");
                return AuthenticateResult.Fail("Unauthorized - No Token Provided");
            }

            var employee = await _redisCacheService.GetValueAsync<EmployeeDto>(token, CancellationToken.None);

            if (employee == null)
            {
                _logger.LogWarning("Unauthorized - No Token Provided");
                return AuthenticateResult.Fail("Unauthorized - No Token Provided");
            }

            bool ValidToken = _tokenService.ValidateToken(token);

            if (!ValidToken)
            {
                _logger.LogWarning("Unauthorized - Invalid Token");
                return AuthenticateResult.Fail("Unauthorized - Invalid Token");
            }

            var principal = CreatePrincipal(employee);
            _claimsPrincipalProvider.Principal = principal;

            Context.User = principal;
            Thread.CurrentPrincipal = principal;
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsPrincipal CreatePrincipal(EmployeeDto employee)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.EmployeeName!),
                new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()!),
                new Claim(ClaimTypes.Role, employee.Role ?? "User"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
