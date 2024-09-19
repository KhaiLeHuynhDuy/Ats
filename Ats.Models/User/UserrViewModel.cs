using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.User
{
    public class UserrViewModel
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? GroupId { get; set; }
        public Active LockoutEnabled { get; set; }
    }
}
