using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.User
{
    public class UserrSearchViewModel : BaseSearchViewModel
    {
        public List<UserrViewModel> Users { get; set; }
        public UserrViewModel User { get; set; }
    }
}
