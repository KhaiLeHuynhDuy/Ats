using Ats.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Models.User
{
    public class UserSearchViewModel
    {
        public UserSearchViewModel()
        {
            UsersProfile = new HashSet<UserProfileModel>();
        }
        public IEnumerable<UserProfileModel> UsersProfile { set; get; }

        public string SearchText { get; set; }
        public int PageIndex { get; set; }
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
    }
}
