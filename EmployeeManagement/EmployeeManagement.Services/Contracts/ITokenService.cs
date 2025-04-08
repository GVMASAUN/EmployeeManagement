using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Services.DTOs;

namespace EmployeeManagement.Services.Contracts
{
    public interface ITokenService
    {
        public string GenerateToken(EmployeeDto employeeDto);
        public bool ValidateToken(string token);
        public string GetEmployeeId(string token);
    }
}
