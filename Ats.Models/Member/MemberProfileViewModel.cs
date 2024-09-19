using Ats.Domain.Lead.Models;
using Ats.Models.Comment;
using Ats.Models.Document;
using Ats.Models.LoanBook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Member
{
   public class MemberProfileViewModel 
    {
        public bool MemberCodeAutomatically { get; set; } 
     
        public int? Month { get; set; }
        public int? Day { get; set; }
      
        public string TierName { get; set; }
        public string City { get; set; }
        public double Point { get; set; }
      
        public string StoreName { get; set; }
   
        public string ChannelName { get; set; }
    
        public string HomeAddress { get; set; }
        public string FullAddress { get; set; }
        public string HalfAddress { get; set; }
       
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public int? DistrictId { get; set; }
        public string District { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string AdditionalAddress { get; set; }
        public string FullAdditionalAddress { get; set; }
        public int? AdditionalProvinceId { get; set; }
        public string AdditionalProvince { get; set; }
        public int? AdditionalDistrictId { get; set; }
        public string AdditionalDistrict { get; set; }
        public List<DocumentViewModel> Documents { get; set; }
        public DocumentViewModel Document { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public CommentViewModel Comment { get; set; }
        public bool TermsOfUse { get; set; }
        public string HomeAddressZipCode { get; set; }
        public string HomeAddressDetails { get; set; }
        public string ReferralMember { get; set; }
        public string ProvinceName { get; set; }
        public int? ProductID { get; set; }
        public string PreferredProduct { get; set; }

        public string PreferredBrand { get; set; }
    }
}
