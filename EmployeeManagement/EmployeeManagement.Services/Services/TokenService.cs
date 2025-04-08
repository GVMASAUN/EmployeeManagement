using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Services.Contracts;
using EmployeeManagement.Services.DTOs;

namespace EmployeeManagement.Services.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(EmployeeDto employeeDto)
        {
            throw new NotImplementedException();
        }

        public string GetEmployeeId(string token)
        {
            throw new NotImplementedException();
        }

        public bool ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
