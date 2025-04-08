using EmployeeManagement.Services.Features;
using EmployeeManagement.WebApi.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebApi.Controllers
{
    [ApiController]
    [Route(RouteKeys.MainRoute)]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(RouteKeys.GetAllEmployees)]
        public async Task<IActionResult> GetAllEmployees(CancellationToken cancellationToken)
        {
            var employees = await _mediator.Send(new GetAllEmployees.GetAllEmployeeQuery(), cancellationToken);

            if (employees == null || employees.Count == 0)
            {
                return Ok(new { Message = "There are no employees." });
            }

            return Ok(new
            {
                Message = "Employees retrived successfully",
                Employees = employees
            });
        }

        [HttpGet(RouteKeys.GetEmployeeById)]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int employeeId, CancellationToken cancellationToken)
        {
            var employee = await _mediator.Send(new GetEmployeeById.GetEmployeeByIdQuery(employeeId), cancellationToken);

            if (employee == null)
            {
                return NotFound(new { Message = $"Employee with ID {employeeId} not found." });
            }
            return Ok(new { Message = "Employee retrieved successfully", Employee = employee });
        }

        [HttpGet(RouteKeys.GetEmployeeByName)]
        public async Task<IActionResult> GetEmployeeByName([FromRoute] string employeeName, CancellationToken cancellationToken)
        {
            var employee = await _mediator.Send(new GetEmployeeByName.GetEmployeeByNameQuery(employeeName), cancellationToken);

            if (employee == null)
            {
                return NotFound(new { Message = $"Employee with Name {employeeName} not found." });
            }
            return Ok(new { Message = "Employee retrieved successfully", Employee = employee });
        }

        [HttpGet(RouteKeys.GetEmployeesByDepartmentId)]
        public async Task<IActionResult> GetEmployeesByDepartmentId([FromRoute] int departmentId, CancellationToken cancellationToken)
        {
            var employees = await _mediator.Send(new GetEmployeeByDepartmentId.GetEmployeeByDepartmentIdQuery(departmentId), cancellationToken);
            if (employees == null || employees.Count == 0)
            {
                return NotFound(new { Message = $"No employees found for Department ID {departmentId}." });
            }
            return Ok(new { Message = "Employees retrieved successfully", Employees = employees });
        }

        [HttpGet(RouteKeys.GetAverageSalary)]
        public async Task<IActionResult> GetAverageSalary(CancellationToken cancellationToken)
        {
            var averageSalary = await _mediator.Send(new GetAverageSalary.GetAverageSalaryQuery(), cancellationToken);
            if (averageSalary == 0)
            {
                return NotFound(new { Message = "No employees found." });
            }
            return Ok(new { Message = "Average salary retrieved successfully", AverageSalary = averageSalary });
        }
    }
}
