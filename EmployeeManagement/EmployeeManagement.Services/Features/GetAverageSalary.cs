using EmployeeManagement.DataAccess.Contracts;
using MediatR;

namespace EmployeeManagement.Services.Features
{
    public class GetAverageSalary
    {
        public class GetAverageSalaryQuery : IRequest<double>
        {
            public GetAverageSalaryQuery() { }
        }

        public class Handler : IRequestHandler<GetAverageSalaryQuery, double>
        {
            private readonly IUnitOfWork _unitOfWork;
            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<double> Handle(GetAverageSalaryQuery request, CancellationToken cancellationToken)
            {
                var averageSalary = await _unitOfWork.EmployeeRepository.GetAverageSalaryAsync(cancellationToken);

                return averageSalary;
            }
        }
    }
}
