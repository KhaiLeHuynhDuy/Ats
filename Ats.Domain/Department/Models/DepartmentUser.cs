using Ats.Domain.Identity.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Departments.Models
{
    public enum DEPARTMENT_ROLE
    {
        [Display(Name = "Undefined")]
        UNDEFINED = 0,
        [Display(Name = "Member")]
        MEMBER = 1,
        [Display(Name = "Owner")]
        OWNER = 2,
        [Display(Name = "Watcher")]
        WATCHER = 3,
        [Display(Name = "Manager")]
        MANAGER = 4
    };

    [Table("departmentusers")]
    public class DepartmentUser : AuditBase, IEntity<System.Guid>
    {
        [Key]
        public System.Guid Id { get; set; }

        public System.Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public DEPARTMENT_ROLE Role { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
