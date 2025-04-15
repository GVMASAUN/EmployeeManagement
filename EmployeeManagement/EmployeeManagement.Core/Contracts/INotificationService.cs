using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Contracts
{
    public interface INotificationService
    {
        Task SendNotificationToAllAsync(string message);
        Task SendNotificationToUserAsync(string userId, string message);
    }
}
