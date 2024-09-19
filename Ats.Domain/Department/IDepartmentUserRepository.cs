using Ats.Domain;
using Ats.Domain.Departments.Models;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;

namespace Ats.Domain.Departments.Repositories
{
    public interface IDepartmentUserRepository : IRepository<DepartmentUser, System.Guid>
    {
        List<User> GetDepartmentUsers(Guid departmentId, int month, int year, int pageIndex, int pageSize);
        List<Guid> GetDepartmentUserIdByDate(Guid departmentId, DateTime dateFrom, DateTime dateTo);
        bool CheckExistAddDepartmentUser(Guid departmentId, Guid userId);
    }
}
