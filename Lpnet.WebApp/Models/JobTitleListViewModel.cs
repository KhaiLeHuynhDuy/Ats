using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpnet.WebApp.Models
{
    public class JobTitleListViewModel
    {
        public string Search { get; set; }
        public SimpleCreationViewModel<int> CreationModel { get; set; }
        public List<SimpleCreationViewModel<int>> Result { get; set; }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}