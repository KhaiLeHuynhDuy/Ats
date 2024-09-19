using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
   public class MemberReportsViewModel
    {
        public Guid Id { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string YOB { get; set; }
        public string CreateDate { get; set; }
        public string RegisterEmployee { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public string TierName { get; set; }
        public double Point { get; set; }
        public string MaleOrFemale { get; set; }
        public string StringStatus { get; set; }

        public bool Active { get; set; }
    }
}
