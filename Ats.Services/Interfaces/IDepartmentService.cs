using Ats.Models.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IDepartmentService
    {
        List<DepartmenttViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        DepartmenttViewModel GetDepartment(Guid id);
        void CreateDepartment(DepartmenttViewModel model);
        void UpdateDepartment(DepartmenttViewModel model);
        void DeleteDepartment(Guid id);
    }
}
