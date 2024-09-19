using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Department
{
    public class DepartmenttSearchViewModel : BaseSearchViewModel
    {
        public List<DepartmenttViewModel> Departments { get; set; }
        public DepartmenttViewModel Department { get; set; }
    }
}
