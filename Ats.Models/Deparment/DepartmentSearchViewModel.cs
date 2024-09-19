using Ats.Models.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Models.Department
{
    public class DepartmentSearchViewModel
    {
        public IEnumerable<DepartmentViewModel> Departments { set; get; }

        public string SearchText { get; set; }
        public int PageIndex { get; set; }
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
    }
}
