using EmployeeManagement.DataAccess.Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;

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
            private readonly ILogger<Handler> _logger;
            public Handler(IUnitOfWork unitOfWork, ILogger<Handler> logger)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
            }
            public async Task<double> Handle(GetAverageSalaryQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var averageSalary = await _unitOfWork.EmployeeRepository.GetAverageSalaryAsync(cancellationToken);

                    if (averageSalary == 0)
                    {
                        _logger.LogWarning("No employees found to calculate average salary");
                        return 0;
                    }

                    return averageSalary;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error fetching average salary: {Message}", exception.Message);
                    throw new Exception("Error fetching average salary.", exception);
                }
            }
        }
    }
}
