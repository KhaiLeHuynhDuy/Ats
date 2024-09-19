using Ats.Domain;
using Ats.Models.Account;
using Ats.Models.Deparment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models.Department
{
    public class DepartmentViewModel
    {
        public DepartmentViewModel()
        {
            Users = new HashSet<UserProfileModel>();
            DepartmentUsers = new HashSet<DepartmentUserModel>();
        }

        [Key]
        public System.Guid Id { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Deparment Code cannot be longer than 16 characters.")]
        public string DepartmentCode { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Deparment Name cannot be longer than 255 characters.")]
        public string DepartmentName { get; set; }

        public string Note { get; set; }
        public bool IsActive { get; set; }

        public Guid ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public UserProfileModel Manager { get; set; }

        public ICollection<UserProfileModel> Users { get; set; }
        public ICollection<DepartmentUserModel> DepartmentUsers { get; set; }
    }
}
