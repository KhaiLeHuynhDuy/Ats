using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpnet.WebApp.Models
{
    public class TeamsViewModel
    {
        public string Search { get; set; }
        public int SelectOrgType { get; set; }
        public SimpleCreationViewModel<Guid> CreationModel { get; set; }
        public List<SimpleCreationViewModel<Guid>> Result { get; set; }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}