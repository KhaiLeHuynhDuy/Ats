using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Team
{
    public class TeamSearchViewModel : BaseSearchViewModel
    {
        public List<TeamViewModel> Teams { get; set; }
        public TeamViewModel Team { get; set; }
    }
}
