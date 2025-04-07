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
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<ICollection<EmployeeDto>> Handle(GetEmployeeByDepartmentIdQuery request, CancellationToken cancellationToken)
            {
                var employees = await _unitOfWork.EmployeeRepository.GetEmployeesByDepartmentIdAsync(request.DepartmentId, cancellationToken);
                return _mapper.Map<ICollection<EmployeeDto>>(employees);
            }
        }
    }
}
