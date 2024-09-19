using Ats.Commons.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.User
{
    public class UserrProfileViewModel
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public Guid? GroupId { get; set; }
        public string GroupName { get; set; }
        public List<UserRolesNameViewModel> Roles { get; set; }
    }
}
