using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.WebApi.Authorization.Attributes
{
    public class AdminAttribute : AuthorizeAttribute
    {
        public AdminAttribute()
        {
            Policy = "AdminPolicy";
        }
    }
}
