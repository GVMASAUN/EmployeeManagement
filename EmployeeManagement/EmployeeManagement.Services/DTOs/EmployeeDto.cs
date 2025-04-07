using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Services.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public required string EmployeeName { get; set; }
        public required string Email { get; set; }
        public long PhoneNumber { get; set; }
        public int Salary { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDto? Department { get; set; }

    }
}
