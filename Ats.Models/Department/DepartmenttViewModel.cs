using Ats.Models.Deparment;
using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Department
{
    public class DepartmenttViewModel
    {
        public DepartmenttViewModel()
        {
            Users = new HashSet<UserrProfileViewModel>();
            DepartmentUsers = new HashSet<DepartmentUserModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid ManagerId { get; set; }
        public UserrProfileViewModel Manager { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserrProfileViewModel> Users { get; set; }
        public ICollection<DepartmentUserModel> DepartmentUsers { get; set; }
    }
}
