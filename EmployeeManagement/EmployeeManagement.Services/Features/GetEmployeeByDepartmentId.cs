using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.Features
{
    public class GetEmployeeByDepartmentId
    {
        public class GetEmployeeByDepartmentIdQuery : IRequest<ICollection<EmployeeDto>>
        {
            public int DepartmentId { get; set; }
            public GetEmployeeByDepartmentIdQuery(int departmentId)
            {
                DepartmentId = departmentId;
            }
        }

        public class Handler : IRequestHandler<GetEmployeeByDepartmentIdQuery, ICollection<EmployeeDto>>
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
            public async Task<ICollection<EmployeeDto>> Handle(GetEmployeeByDepartmentIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var employees = await _unitOfWork.EmployeeRepository.GetEmployeesByDepartmentIdAsync(request.DepartmentId, cancellationToken);
                    if (employees == null || employees.Count == 0)
                    {
                        _logger.LogWarning("No employees found for department ID {DepartmentId}", request.DepartmentId);
                        return new List<EmployeeDto>();
                    }
                    return _mapper.Map<ICollection<EmployeeDto>>(employees);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error fetching employees by department ID: {Message}", exception.Message);
                    throw new Exception("Error fetching employees.", exception);
                }
            }
        }
    }
}
