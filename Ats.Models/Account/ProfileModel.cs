using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Models.Account
{
    public class ProfileModel
    {
        public System.Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime DOB { get; set; }

        public string UserId { get; set; }        
        public UserProfileModel User { get; set; }

        public string AvatarPhotoUrl { get; set; }

        public string AvatarPhoto { get; set; }
        public string HighlightColor { get; set; }

        public int UserType { get; set; }

        public bool IsDeleted { get; set; }

        
        public string FullName {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
