using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Models.Account
{
    public class RoleGroupSearchViewModel
    {
        public RoleGroupSearchViewModel()
        {
            RoleGroups = new HashSet<GroupViewModel>();
        }
        public string SearchText { get; set; }
        public IEnumerable<GroupViewModel> RoleGroups { set; get; }
        public int PageIndex { get; set; }
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
    }
}
