using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Lead.Models;

namespace Ats.Models.Member
{
   public class MemberSearchProfileViewModel
    {
       
        public bool MemberCodeAutomatically { get; set; }
       
        public int? Month { get; set; }
        public int? Day { get; set; }
     
        public string TierName { get; set; }

        public string City { get; set; }
        public double Point { get; set; }
       
        public string StoreName { get; set; }
     
        public string ChannelName { get; set; }
        public Guid? RegisterEmployee { get; set; }
        public string JobTitle { get; set; }
        public string Occupation { get; set; }
        public string HomeAddress { get; set; }
        public string FullAddress { get; set; }
        public string HalfAddress { get; set; }
        public string Province { get; set; }    
        public string District { get; set; }
        public string CountryName { get; set; }
        public string AdditionalAddress { get; set; }
        public string AdditionalProvince { get; set; }
        public string AdditionalDistrict { get; set; }
        public string FullAdditionalAddress { get; set; }
        public string HomeAddressZipCode { get; set; }
        public string HomeAddressDetails { get; set; }
        public string ReferralMember { get; set; }
        public string ProvinceName { get; set; }
        public int? ProductID { get; set; }
        public string PreferredProduct { get; set; }

        public string PreferredBrand { get; set; }
        public bool TermsOfUse { get; set; }

    }
}
