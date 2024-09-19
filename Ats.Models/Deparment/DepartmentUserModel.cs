using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Deparment
{
    public class DepartmentUserModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Tag { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
