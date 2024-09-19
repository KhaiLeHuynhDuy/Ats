using Ats.Commons.Models;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Account;
using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ats.Services.Interfaces
{
    public interface IAccountService : IBaseService
    {
        IEnumerable<DisplayItem> GetActiveUsers();
        IQueryable<User> GetAllUsers();
        IEnumerable<UserProfileModel> Search();
        UserSearchViewModel SearchWithPaging(string searchText, int pageIndex);
        UserProfileModel GetUser(Guid id);
        Task<BaseResponse<UserProfileModel>> CreateUser(UserProfileModel model);
        Task<BaseResponse<UserProfileModel>> UpdateUserProfile(UserProfileModel model);
        string EditUserProfile(UserProfileModel model);       
        IEnumerable<GroupViewModel> GetAllRoleGroups();
        GroupViewModel GetRoleGroup(Guid groupId);
        RoleGroupSearchViewModel GetRoleGroupWithPaging(string searchText, int pageIndex);
        Task<BaseResponse<GroupViewModel>> UpdateRoleGroup(GroupViewModel model);
        List<UserRolesNameViewModel> GetUserRoles(Guid userId);
        //List<String> GetUserRoles(string userId);
        float GetTargetHours(Guid userId, DateTime month);
        void DeleteUser(Guid id);
        void DeleteGroup(Guid id);
        Task<BaseResponse<GroupViewModel>> CreateRoleGroup(GroupViewModel model);
        ICollection<DisplayItem> GetRoles(List<Guid> selectedIds);        
    }
}