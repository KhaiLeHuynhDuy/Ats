using Ats.Domain.Departments.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Department
{
    public class DepartmentUserEditModel
    {
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid UserId { get; set; }
        public DEPARTMENT_ROLE Role { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem> userList { get; set; }

    }
}
