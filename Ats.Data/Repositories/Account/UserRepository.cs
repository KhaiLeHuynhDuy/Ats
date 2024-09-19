using Ats.Commons.Models;
using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Data.Accounts.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(SCDataContext context) : base(context)
        {
        }

        public IEnumerable<User> Search()
        {
            //var profile = this.GetAll().Where(x => (!String.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
            var users = this.GetAll().OrderBy(x => x.FirstName);
            return users;
        }

        public void SetRoleGroup(Guid userId, Guid groupId)
        {
            var user = this.GetById(userId);

            if (!user.UserGroups.Any(x => x.GroupId == groupId))
            {
                var userGroups = user.UserGroups.ToList();

                foreach (var item in userGroups)
                {
                    user.UserGroups.Remove(item);
                }

                Group rg = this.DataContext.Get<Group>().Where(x => x.Id == groupId).FirstOrDefault();

                if (rg != null)
                {
                    user.UserGroups.Add(new UserGroup() { UserId = userId, GroupId = rg.Id });

                    List<string> roles = new List<string>();
                    foreach (var g in user.UserGroups)
                    {
                        roles.AddRange(g.Group.GroupRoles.Select(x => x.Role.Name));
                    }

                    var allRoles = this.DataContext.Get<Role>().Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToList();
                    var existingRoles = user.UserRoles.Select(x => x.RoleId).ToList();

                    // added roles
                    allRoles.Where(x => !existingRoles.Contains(x)).ToList().ForEach(x => user.UserRoles.Add(new UserRole() { UserId = user.Id, RoleId = x }));

                    // remove
                    user.UserRoles.Where(x => !allRoles.Contains(x.RoleId)).ToList().ForEach(x => user.UserRoles.Remove(x));

                }
            }
        }

        public Group GetRoleGroup(Guid userId)
        {
            var user = this.GetById(userId);
            if(user.UserGroups.Count > 0)
            {
               return user.UserGroups.FirstOrDefault().Group;
            }
            return null;
        }

        public void UpdateRoles(Guid groupId)
        {
            var group = this.DataContext.Get<Group>().Where(x => x.Id == groupId).FirstOrDefault();
            var users = group.UserGroups.Select(x => x.User);

            foreach (var user in users)
            {
                List<string> roles = new List<string>();

                foreach (var g in user.UserGroups.Select(x => x.Group))
                {
                    roles.AddRange(g.GroupRoles.Select(x => x.Role.Name));
                }

                var allRoles = this.DataContext.Get<Role>().Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToList();
                var existingRoles = user.UserRoles.Select(x => x.RoleId).ToList();

                // added roles
                allRoles.Where(x => !existingRoles.Contains(x)).ToList().ForEach(x => user.UserRoles.Add(new UserRole() { UserId = user.Id, RoleId = x }));

                // remove
                user.UserRoles.Where(x => !allRoles.Contains(x.RoleId)).ToList().ForEach(x => user.UserRoles.Remove(x));
            }
        }
        public void UpdateRolesVer2(Guid groupId, IEnumerable<string> selectedRoles)
        {
            var User = DataContext.Set<UserGroup>().Where( x => x.GroupId == groupId).Select(x=>x.User.Id);
            if (User != null)
            {
                var UserRoleRemove = DataContext.Set<UserRole>().Where(x => User.Contains(x.UserId));
                DataContext.Set<UserRole>().RemoveRange(UserRoleRemove);

                var IdRole = DataContext.Set<Role>().Where(x => selectedRoles.Contains(x.Name)).ToList();
                List<UserRole> listUserRoleAdd = new List<UserRole>();
                foreach (var userid in User.ToList())
                {
                    foreach (var item in IdRole)
                    {
                        listUserRoleAdd.Add(new UserRole { UserId = userid, RoleId = item.Id });
                    }
                }

                DataContext.Set<UserRole>().AddRange(listUserRoleAdd);
            }
        }

        public IEnumerable<UserRolesNameViewModel> GetUserRoles(Guid userId)
        {
            //var user = this.GetById(userId);
            //var xxx = this.DataContext.Get<UserRole>().Where(x => x.UserId == user.Id).Select(x => x.RoleId);

            //var userRoles = user.UserRoles.Select(x => x.RoleId);


            var query = (from user in DataContext.User
                         join userRole in DataContext.UserRole on user.Id equals userRole.UserId
                          join role in DataContext.Role on userRole.RoleId equals role.Id
                          where user.Id == userId
                          select new UserRolesNameViewModel()
                          {
                             Name = role.Name
                          }).AsEnumerable();
            return query;

        }
        
        public DbSet<UserRole> GetUserRoleRepo()
        {
            return DataContext.Set<UserRole>();
        }        

        public List<User> GetUserByDepartment(string searchText, Guid departmentId, int month, int year, int pageIndex, int pageSize, out int total)
        {
            List<Guid> users = new List<Guid>();
            if (departmentId != Guid.Empty)
            {
                DateTime date1 = new DateTime(year, month, 1);
                DateTime date2 = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                users = (from du in DataContext.DepartmentUser
                         where du.DepartmentId.Equals(departmentId)
                         && (du.StartDate <= date2 && (!du.EndDate.HasValue || (du.EndDate.HasValue && du.EndDate.Value >= date1)))
                         select du.UserId).ToList();
            }

            var query = (from s in DataContext.User
                         where (
                         (departmentId == Guid.Empty || (users.Contains(s.Id)))
                         &&
                         (String.IsNullOrEmpty(searchText)
                         || (s.FirstName != null || s.LastName != null) && ((s.FirstName.ToLower() + " " + s.LastName.ToLower()).ToLower().Contains(searchText)
                         || (s.LastName.ToLower() + " " + s.FirstName.ToLower()).ToLower().Contains(searchText))
                         || s.Email != null && s.Email.ToLower().Contains(searchText)                         
                         || s.Title != null && s.Title.ToLower().Contains(searchText)))
                         select new User
                         {
                             Id = s.Id,
                             FirstName = s.FirstName,
                             LastName = s.LastName,
                             Email = s.Email,
                             Title = s.Title                             
                         });

            total = query.Count();
            var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }

        public Guid GetUserId(string username)
        {
            Guid userId = this.GetAll().Where(x=>String.Equals(username, x.UserName, StringComparison.OrdinalIgnoreCase)).Select(x=>x.Id).FirstOrDefault();
            return userId;
        }
    }
}
