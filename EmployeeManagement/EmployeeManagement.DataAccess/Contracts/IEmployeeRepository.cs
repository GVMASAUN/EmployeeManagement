using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Entities;

namespace EmployeeManagement.DataAccess.Contracts
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<Employee?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
        Task<double> GetAverageSalaryAsync(CancellationToken cancellationToken);
        Task<ICollection<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken);
    }
}
