using Ats.Commons.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ats.Models.Account
{
    public class UserProfileModel
    {
       
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*",
         ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [RegularExpression(@"^([0-9]{1,11})$", ErrorMessage = "Please enter a valid Phone Number")]
        [StringLength(30, MinimumLength = 10, ErrorMessage = "Please enter a valid Phone Number")]
        public string PhoneNumber { get; set; }

        public string Tag { get; set; }

        public Guid GroupId { get; set; }
        public string GroupName { get; set; }

        public Active LockoutEnabled { get; set; }
        public bool IsActive { get; set; }       
        public List<UserRolesNameViewModel> Roles { get; set; }

        public string FullName {
            get {
                string fullname = this.FirstName;
                if(!String.IsNullOrEmpty(this.LastName))
                {
                    return String.Format("{0}, {1}", this.FirstName, this.LastName);
                }
                return fullname;
            }
        }

        public bool IsSystemAdminGroup
        {
            get
            {
                return !string.IsNullOrEmpty(GroupName) && GroupName.Contains(Ats.Commons.Constants.SYS_ROLE_SYSTEM_ADMINISTRATION);
            }
        }       

        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartDate  { get; set; }

        [DisplayName("Joining Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? JoiningDate  { get; set; }

        [DisplayName("Resignation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ResignationDate { get; set; }

        [Required]
        public string UserCode { get; set; }
        public string DeviceUserID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { set; get; }

        //public string AttachmentFileName { set; get; }

        //public HttpPostedFileBase File { get; set; }
        public IFormFile File { get; set; }

        [RegularExpression(@"^([0-9]{1,11})$", ErrorMessage = "Please enter a valid Phone Number")]
        [StringLength(30, MinimumLength = 10, ErrorMessage = "Please enter a valid Phone Number")]
        public string AdditionalPhone { get; set; }

        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*",
        ErrorMessage = "Please enter a valid email address")]
        public string AdditionalEmail { get; set; }
        public string Address { get; set; }

        [DisplayName("Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        public string Skype { get; set; }

        public string Project { get; set; }
    }
}
