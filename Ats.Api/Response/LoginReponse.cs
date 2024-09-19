using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Api.Response
{
    public class LoginReponse
    {
        public LoginReponse()
        {
            UserId = Guid.Empty;
            AccessToken = "";
            FirstName = "";
            LastName = "";
            Email = "";

            Address = "";
            UserCode = "";
            Phone = "";
            Skype = "";
            AttachmentLink = "";
            GroupName = "";
            DateOfBirth = default;
            Project = default;
            Mobile = "(029)555-0104";
        }
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserCode { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string AttachmentLink { get; set; }
        public string GroupName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Array Project { get; set; }
        public string Mobile { get; set; }
    }
}
