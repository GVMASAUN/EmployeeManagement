using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Services.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
    }
}
