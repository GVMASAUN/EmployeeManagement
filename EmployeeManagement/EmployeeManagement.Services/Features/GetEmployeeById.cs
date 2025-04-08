using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Features
{
    public class GetEmployeeById
    {
        public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
        {
            public int EmployeeId { get; set; }
            public GetEmployeeByIdQuery(int employeeId)
            {
                EmployeeId = employeeId;
            }
        }

        public class Handler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            public Handler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Handler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.EmployeeId, cancellationToken);
                    if (employee == null)
                    {
                        _logger.LogWarning("Employee with ID {Id} not found", request.EmployeeId);
                        return null;
                    }
                    return _mapper.Map<EmployeeDto>(employee);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error fetching employee by Id: {Message}", exception.Message);
                    throw new Exception("Error fetching employee.", exception);
                }
            }
        }
    }
}
