using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Entities;
using EmployeeManagement.Services.DTOs;
using MapsterMapper;
using MediatR;
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
            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ICollection<EmployeeDto>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    ICollection<Employee> employees = await _unitOfWork.EmployeeRepository.GetAllEmployeeAsync(cancellationToken);

                    ICollection<EmployeeDto> employeesDto = _mapper.Map<ICollection<EmployeeDto>>(employees);

                    return employeesDto;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<EmployeeDto>();
                }
            }
        }
    }
}
