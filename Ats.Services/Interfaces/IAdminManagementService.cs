using Ats.Domain.Departments.Models;
using Ats.Models;
using Ats.Models.Account;
using Ats.Models.Department;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Ats.Services.Interfaces
{
    public interface IAdminManagementService : IBaseService
    {        
        IEnumerable<DisplayItem> GetActiveDepartMents();
        IEnumerable<DepartmentViewModel> SearchDepartment();
        DepartmentSearchViewModel SearchDepartmentWithPaging(string searchText, int pageIndex);
        DepartmentViewModel GetDepartment(Guid id);
        DepartmentViewModel GetDepartmentFilter(Guid id, DateTime startDate, DateTime endDate);
        Guid CreateDepartment(DepartmentViewModel model);
        void UpdateDepartment(DepartmentViewModel model);
        Guid AddDepartmentUser(Guid departmentId, Guid userId, DEPARTMENT_ROLE role, DateTime startDate, DateTime? endDate);
        bool CheckExistAddDepartmentUser(Guid departmentId, Guid userId);
        DepartmentUserEditModel GetDepartmentUserDetail(Guid id);
        void UpdateDepartmentUser(DepartmentUserEditModel model);
        void DeleteDepartmentUser(Guid departmentId, Guid userId);
        IEnumerable<DisplayItem> GetUserManagementDepartment(Guid userId);
        IEnumerable<DisplayItem> GetUserManagementDepartmentFilter(Guid userId);
        List<SelectListItem> GetUserManagementDepartmentListItem(Guid userId);
        IEnumerable<DisplayItem> GetDepartmentUser(Guid userId);
        bool isManagerDepartment(Guid userId, Guid departmentId);
        void DeleteDerpartment(Guid id);
        IEnumerable<DisplayItem> GetNotSelectedDepartmentUser(Guid departId);
        List<DepartmentUser> GetAllDepartmentManagers(Guid userId);
        ResponseViewModel<List<UserProfileModel>> GetDepartmentWithPaging(Guid departmentId, string searchText, int pageIndex);
        ResponseViewModel<List<UserProfileModel>> GetDepartmentWithPagingFilter(Guid departmentId, string searchText, int pageIndex, DateTime StartDate, DateTime EndDate);
    }
}