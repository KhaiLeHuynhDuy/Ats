using Ats.Domain;
using System;
using System.Collections.Generic;
using Ats.Domain.Identity.Models;

namespace Ats.Domain.Accounts.Repositories
{
    public interface IGroupRepository : IRepository<Group, Guid>
    {
        void UpdateGroup(Guid id, String name, String desc, IEnumerable<String> selectedRoles);
        IEnumerable<Role> GetRolesByGroup(Guid groupId);
        void DeleteGroup(Guid groupId);
    }
}
