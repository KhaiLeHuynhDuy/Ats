using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IUserService
    {
        //User
        List<UserrViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        UserrProfileViewModel GetUser(Guid id);
        void CreateUser(UserrViewModel model);
        void UpdateUser(UserrProfileViewModel model);
        void DeleteUser(Guid id);
        bool CheckExistUserNameOrEmail(UserrViewModel model);
        //Group
        List<RoleGrouppViewModel> SearchRoleGroup(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        RoleGrouppViewModel GetRoleGroup(Guid id);
        void CreateRoleGroup(RoleGrouppViewModel model);
        void DeleteRoleGroup(Guid id);
    }
}
