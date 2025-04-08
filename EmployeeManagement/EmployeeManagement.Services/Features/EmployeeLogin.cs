using EmployeeManagement.Core.Contracts;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Features
{
    public class EmployeeLogin
    {
        public class EmployeeLoginQuery : IRequest<LoginDto>
        {
            public LoginDto LoginDto { get; set; }

            public EmployeeLoginQuery(LoginDto loginDto)
            {
                LoginDto = loginDto;
            }
        }

        public class Handler : IRequestHandler<EmployeeLoginQuery, LoginDto>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ITokenService _tokenService;
            private readonly IRedisCacheService _redisCacheService;
            private readonly IMapper _mapper;

            public Handler(ILogger<Handler> logger, IUnitOfWork unitOfWork, ITokenService tokenService, IRedisCacheService redisCacheService, IMapper mapper)
            {
                _logger = logger;
                _unitOfWork = unitOfWork;
                _tokenService = tokenService;
                _redisCacheService = redisCacheService;
                _mapper = mapper;
            }

            public async Task<LoginDto> Handle(EmployeeLoginQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    //var employeeToken = request.LoginDto.Token;
                    request.LoginDto.Token = null;
                    var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByEmailAsync(request.LoginDto.Email, cancellationToken);
                    if (employee == null)
                    {
                        _logger.LogWarning("Employee with email {Email} not found", request.LoginDto.Email);
                        return request.LoginDto;
                    }
                    _logger.LogInformation($"Db:{employee.Password}\napi{BCrypt.Net.BCrypt.HashPassword(request.LoginDto.Password)}");

                    if (BCrypt.Net.BCrypt.Verify(request.LoginDto.Password, employee.Password))
                    {
                        EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
                        string token = _tokenService.GenerateToken(employeeDto);
                        await _redisCacheService.SetValueAsync(token, employeeDto, TimeSpan.FromHours(1), cancellationToken);

                        _logger.LogInformation($"{employeeDto.EmployeeName} logged in successfully");
                        request.LoginDto.Token = token;
                        return request.LoginDto;
                    }
                    else
                    {
                        _logger.LogWarning("Invalid password for employee with email {Email}", request.LoginDto.Email);
                        return request.LoginDto;
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error during employee login: {Message}", exception.Message);
                    throw new Exception("Error during employee login.", exception);
                }
            }
        }
    }
}
