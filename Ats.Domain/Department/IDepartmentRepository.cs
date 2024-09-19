using Ats.Domain;
using Ats.Domain.Departments.Models;
using System;
using System.Collections.Generic;

namespace Ats.Domain.Departments.Repositories
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
        IEnumerable<Department> Search();
    }
}
