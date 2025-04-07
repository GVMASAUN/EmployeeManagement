using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;

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
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<EmployeeDto?> Handle(GetEmployeeByNameQuery request, CancellationToken cancellationToken)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByNameAsync(request.EmployeeName, cancellationToken);

                if (employee == null)
                {
                    return null;
                }
                return _mapper.Map<EmployeeDto>(employee);
            }
        }
    }
}
