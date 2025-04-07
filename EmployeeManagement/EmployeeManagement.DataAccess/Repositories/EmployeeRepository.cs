using EmployeeManagement.Contexts;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DataAccess.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseContext _context;

        public EmployeeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Employee>> GetEmployeesByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken)
        {
            return await _context.Employees.Include(employee => employee.Department)
                .Where(employee => employee.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<ICollection<Employee>> GetAllEmployeeAsync(CancellationToken cancellationToken)
        {
            return await _context.Employees.Include(employee => employee.Department)
                .ToListAsync();
        }

        public async Task<double> GetAverageSalaryAsync(CancellationToken cancellationToken)
        {
            var anyEmployees = await _context.Employees.AnyAsync();
            if (!anyEmployees)
            {
                return 0.0;
            }
            return await _context.Employees.AverageAsync(employee => employee.Salary);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _context.Employees.Include(employee => employee.Department)
                .FirstOrDefaultAsync(employee => employee.EmployeeId == employeeId);
        }

        public async Task<Employee?> GetEmployeeByNameAsync(string employeeName, CancellationToken cancellationToken)
        {
            return await _context.Employees.Include(employee => employee.Department)
                .FirstOrDefaultAsync(employee => employee.EmployeeName == employeeName);
        }
    }
}
