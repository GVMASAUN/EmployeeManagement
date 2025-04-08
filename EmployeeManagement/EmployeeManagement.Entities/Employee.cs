using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public required string EmployeeName { get; set; }
        public required long PhoneNumber {  get; set; }
        public required string Email { get; set; }
        public int Salary { get; set; }
        public string? Role { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
