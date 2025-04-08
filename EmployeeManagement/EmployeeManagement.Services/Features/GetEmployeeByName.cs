using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Features
{
    public class GetEmployeeByName
    {
        public class GetEmployeeByNameQuery : IRequest<EmployeeDto>
        {
            public string EmployeeName { get; set; }
            public GetEmployeeByNameQuery(string employeeName)
            {
                EmployeeName = employeeName;
            }
        }

        public class Handler : IRequestHandler<GetEmployeeByNameQuery, EmployeeDto?>
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
            public async Task<EmployeeDto?> Handle(GetEmployeeByNameQuery request, CancellationToken cancellationToken)
            {

                try
                {
                    var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByNameAsync(request.EmployeeName, cancellationToken);

                    if (employee == null)
                    {
                        _logger.LogWarning("Employee with name {Name} not found", request.EmployeeName);
                        return null;
                    }
                    return _mapper.Map<EmployeeDto>(employee);
                }
                catch (Exception exception)
                {

                    _logger.LogError(exception, "Error fetching employee by name: {Message}", exception.Message);
                    throw new Exception("Error fetching employee.", exception);
                }
            }
        }
    }
}
