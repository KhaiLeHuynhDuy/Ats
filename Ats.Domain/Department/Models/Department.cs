using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Departments.Models
{
    [Table("departments")]
    public class Department : AuditBase, IEntity<System.Guid>
    {
        public Department()
        {
            Users = new HashSet<DepartmentUser>();
        }

        [Key]
        public System.Guid Id { get; set; }

        [StringLength(128, ErrorMessage = "Deparment Code cannot be longer than 16 characters.")]
        public string DepartmentCode { get; set; }

        [StringLength(255, ErrorMessage = "Deparment Name cannot be longer than 255 characters.")]
        public string DepartmentName { get; set; }

        public string Note { get; set; }
        public bool IsActive { get; set; }

        public Guid ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual User Manager { get; set; }

        public virtual ICollection<DepartmentUser> Users { get; set; }
    }
}
