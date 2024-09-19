using Ats.Domain;
using Ats.Domain.Departments.Models;
using Ats.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models.Department
{
    public class DepartmentUserSelectionViewModel<T> : SelectedItemViewModel<T>
    {
        public DEPARTMENT_ROLE Role { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? EndDate { get; set; }
    }
}
