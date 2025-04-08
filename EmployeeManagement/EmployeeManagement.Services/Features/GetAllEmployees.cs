using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Entities;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
namespace EmployeeManagement.Services.Features
{
    public class GetAllEmployees
    {
        public class GetAllEmployeeQuery : IRequest<ICollection<EmployeeDto>>
        {
            public GetAllEmployeeQuery() { }
        }

        public class Handler : IRequestHandler<GetAllEmployeeQuery, ICollection<EmployeeDto>>
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

            public async Task<ICollection<EmployeeDto>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    ICollection<Employee> employees = await _unitOfWork.EmployeeRepository.GetAllEmployeeAsync(cancellationToken);
                    if (employees == null || employees.Count == 0)
                    {
                        _logger.LogWarning("No employees found");
                        return new List<EmployeeDto>();
                    }

                    ICollection<EmployeeDto> employeesDto = _mapper.Map<ICollection<EmployeeDto>>(employees);
                    return employeesDto;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error fetching all employees: {Message}", exception.Message);
                    throw new Exception("Error fetching all employees.", exception);
                }
            }
        }
    }
}
