using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Web.Models
{
    public class ProjRoleActiViewModel
    {
        public string ProjId { set; get; }
        public List<string> RolesId { set; get; }
        public List<string> ActiesId { set; get; }
    }
}