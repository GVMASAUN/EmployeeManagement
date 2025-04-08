namespace EmployeeManagement.WebApi.Constants
{
    public class RouteKeys
    {
        //Base Route
        public const string MainRoute = "api/EmployeeManagement";

        public const string EmployeeLogin = "Login";
        public const string GetAllEmployees = "GetEmployees";
        public const string GetEmployeeById = "GetEmployeeById/{employeeId}";
        public const string GetEmployeeByName = "GetEmployeeByName/{employeeName}";
        public const string GetEmployeesByDepartmentId = "GetEmployeesByDepartmentId/{departmentId}";
        public const string GetAverageSalary = "GetAverageSalary";
    }
}
