using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Domain;
using Ats.Domain.Lead.Models;

namespace Ats.Models.Member
{
    public class MemberViewModel
    {

        //public bool MemberCodeAutomatically { get; set; }

        public Guid Id { get; set; }
        public string MemberCode { get; set; }
        public int [] isCheckd { get; set; }
        public string FirstName { get; set; }
        public bool IsSelected { set; get; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public int YOB { get; set; }
        public string StringYOB { get; set; }

        public int? Month { get; set; }
        public int? Day { get; set; }
        public DateTime? DOB { get; set; }
        public string DOBs { get; set; }

        public int? JobTitleId { get; set; }     
        public string Address1 { get; set; }
        public string Area { get; set; }      
        public string DateCreate { get; set; }      
    
        public string PreferredBrand { get; set; }
       
        public MARITAL_STATUS? MaritalStatus { get; set; }
        public DateTime? WeddingDate { get; set; }
        public bool AcceptToReceiveMarketing { get; set; }
        public bool AcceptToReceiveLoyaltyInformation { get; set; }
        public bool AcceptToBeContactedViaMobilePhone { get; set; }
        public bool AcceptToBeContactedViaMobileEmail { get; set; }
        public bool NotAcceptAnyContact { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string RegisterEmployee { get; set; }

        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }

        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }

        public int? AdditionalProvinceId { get; set; }
        public int? AdditionalDistrictId { get; set; }
        //public bool TermsOfUse { get; set; }

        public int? StoreId { get; set; }
        public int? ChannelId { get; set; }
        public int? OccupationId { get; set; }

        public string JobTitleName { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }

        public string ChannelName { get; set; }        

        public int? OriginalFileId { get; set; }
        public int? OriginalFileAdditionId { get; set; }

        public int Tier { get; set; }
        public Guid Tiers { get; set; }
        public string RandomCode { get; set; }
        public string TierName { get; set; }
        public Guid MemberId { get; set; }
        public double Point { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
        public string Remark { get; set; }

    }
}
