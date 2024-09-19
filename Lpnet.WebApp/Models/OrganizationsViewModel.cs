using Ats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpnet.WebApp.Models
{
    public class OrganizationsViewModel
    {
        public string Search { get; set; }
        public int SelectOrgType { get; set; }
        public SimpleCreationViewModel<Guid> CreationModel { get; set; }
        public List<SimpleCreationViewModel<Guid>> Result { get; set; }

        public PagerViewModel Pager { get; set; }
    }
}