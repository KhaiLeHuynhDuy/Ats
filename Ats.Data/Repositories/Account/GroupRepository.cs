using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Data.Accounts.Repositories
{
    public class GroupRepository : Repository<Group, Guid>, IGroupRepository
    {
        public GroupRepository(SCDataContext context) : base(context)
        {
        }

      public void UpdateGroup(Guid id, string name, string desc, IEnumerable<string> selectedRoles)
        {
            var entity = this.GetById(id);

            if (entity == null)
            {
                throw new ApplicationException($"Role Group {id} does not exist.");
            }

            entity.Name = name;
            entity.Description = desc;

            if (selectedRoles == null)
                selectedRoles = new List<string>();
           
            var GroupRoleRemove = DataContext.Set<GroupRole>().Where(x => x.GroupId==id);
            DataContext.Set<GroupRole>().RemoveRange(GroupRoleRemove);

            var IdRole = DataContext.Set<Role>().Where(x => selectedRoles.Contains(x.Name)).ToList();
            //entity.GroupRoles.Where(x => !selectedRoles.Contains(x.Role.Name)).ToList().ForEach(gr => entity.GroupRoles.Remove(gr));

            //var existingRoles = entity.GroupRoles.Select(m => m.Role.Name);

            // var l = this.DataContext.Get<Role>().Where(m => selectedRoles.Except(existingRoles).Contains(m.Name)).ToList();
            //l.ForEach(role => entity.GroupRoles.Add(new GroupRole { GroupId = id, RoleId = role.Id }));
            List<GroupRole> listRoleGroupAdd = new List<GroupRole>();
            foreach(var item in IdRole)
            {
                listRoleGroupAdd.Add(new GroupRole { GroupId = id, RoleId = item.Id });
            }
            DataContext.Set<GroupRole>().AddRange(listRoleGroupAdd);
            //IdRole.ForEach(role => entity.GroupRoles.Add(new GroupRole { GroupId = id, RoleId = role.Id }));

            this.Update(entity, x => x.Name, x => x.Description);
        }

        public IEnumerable<Role> GetRolesByGroup(Guid groupId)
        {
            return this.DataContext.Get<GroupRole>().Where(x => x.GroupId == groupId).Select(x => x.Role);
        }

        public void DeleteGroup(Guid groupId)
        {
            var group = this.GetById(groupId);
          
            var users = group.UserGroups.Select(x => x.User);         

            foreach (User user in users)
            {
                user.UserRoles.ToList().ForEach(x => this.DataContext.Delete<UserRole>(x));              
            }
            this.Delete(group);
        }
    }
}
