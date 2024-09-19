using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberExcelViewModel
    {
        public string MemberCode { get; set; }//giud
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SEX { get; set; }
        public string Phone { get; set; }
        public string YOB { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string JobTitle { get; set; }
        public string Occupation { get; set; }

        public int LoyaltyTier { get; set; }
        public double LoyaltyPoint { get; set; }

        public string Address1 { get; set; }
        public string Province { get; set; }
        public int? ProvinceId { get; set; }

        public string District { get; set; }
        public int? DistrictId { get; set; }

        public string Status { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Guid? RegisterEmployee { get; set; }
        public string REGISTERCHANNEL { get; set; }
        public string REGISTERSTORE { get; set; }
    }
}
