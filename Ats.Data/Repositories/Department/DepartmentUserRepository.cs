using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Departments.Models;
using Ats.Domain.Departments.Repositories;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Data.Departments.Repositories
{
    public class DepartmentUserRepository : Repository<DepartmentUser, Guid>, IDepartmentUserRepository
    {
        public DepartmentUserRepository(SCDataContext context) : base(context)
        {
        }


        public List<User> GetDepartmentUsers(Guid departmentId, int month, int year, int pageIndex, int pageSize)
        {            
            DateTime date1 = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime date2 = new DateTime(year, month, 1);

            return this.GetAll().Where(x => x.DepartmentId == departmentId 
                                        && x.StartDate <= date1 && (!x.EndDate.HasValue || (x.EndDate.HasValue && x.EndDate > date2)))
                                        .Select(x => x.User).OrderBy(x=>x.FirstName).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Guid> GetDepartmentUserIdByDate(Guid departmentId, DateTime dateFrom, DateTime dateTo)
        {
            return this.GetAll().Where(x => x.DepartmentId == departmentId
                                        && x.StartDate <= dateFrom && (!x.EndDate.HasValue || (x.EndDate.HasValue && x.EndDate > dateTo)))
                                        .Select(x => x.UserId).ToList();
        }

        public bool CheckExistAddDepartmentUser(Guid departmentId, Guid userId)
        {
            return this.GetAll().Any(x => x.DepartmentId.Equals(departmentId) && x.UserId.Equals(userId));
        }
    }
}
