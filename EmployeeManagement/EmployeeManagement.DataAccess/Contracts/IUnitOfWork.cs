﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
    }
}
