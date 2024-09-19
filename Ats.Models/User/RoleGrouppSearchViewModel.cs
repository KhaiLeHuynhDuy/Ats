using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.User
{
    public class RoleGrouppSearchViewModel : BaseSearchViewModel
    {
        public List<RoleGrouppViewModel> RoleGroups { get; set; }
        public RoleGrouppViewModel RoleGroup { get; set; }
    }
}
