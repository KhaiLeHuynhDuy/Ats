using Ats.Domain;
using System;
using System.Collections.Generic;
using Ats.Domain.Identity.Models;
using Ats.Commons.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ats.Domain.Accounts.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        IEnumerable<User> Search();
        void SetRoleGroup(Guid userId, Guid roleGroupId);
        Group GetRoleGroup(Guid userId);
        void UpdateRoles(Guid groupId);
        void UpdateRolesVer2(Guid groupId, IEnumerable<string> selectedRoles);
        IEnumerable<UserRolesNameViewModel> GetUserRoles(Guid userId);
        
        DbSet<UserRole> GetUserRoleRepo();

        List<User> GetUserByDepartment(string searchText, Guid departmentId, int month, int year, int pageIndex, int pageSize, out int total);

        Guid GetUserId(string username);
    }
}
