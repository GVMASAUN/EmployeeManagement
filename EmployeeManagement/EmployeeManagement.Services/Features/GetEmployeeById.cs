using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;

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
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
            {
                var employee = await _unitOfWork.EmployeeRepository.GetEmployeeByIdAsync(request.EmployeeId, cancellationToken);
                if (employee == null)
                {
                    return null;
                }
                return _mapper.Map<EmployeeDto>(employee);
            }
        }
    }
}
