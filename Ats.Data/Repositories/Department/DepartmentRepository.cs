using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Departments.Models;
using Ats.Domain.Departments.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Data.Departments.Repositories
{
    public class DepartmentRepository : Repository<Department, Guid>, IDepartmentRepository
    {
        public DepartmentRepository(SCDataContext context) : base(context)
        {
        }

        public IEnumerable<Department> Search()
        {
            var entities = this.GetAll().OrderBy(x => x.DepartmentCode);
            return entities;
        }
    }
}
