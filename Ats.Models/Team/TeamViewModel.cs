using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Team
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            TeamUsers = new HashSet<TeamUserModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<TeamUserModel> TeamUsers { get; set; }
    }
}
