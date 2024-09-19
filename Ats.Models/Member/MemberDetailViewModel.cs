using Ats.Domain;
using Ats.Domain.Lead.Models;
using Ats.Models.Brand;
using Ats.Models.Comment;
using Ats.Models.Document;
using Ats.Models.MemberWallet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ats.Models.Member
{
    public class MemberDetailViewModel 
    {
        //public string City { get; set; }

        public Guid Id { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string FullName{get; set;}
        public string Profile{get; set;}
        public string ZaloId { get; set; }
        //Registration information
        public string StoreName { get; set; }
        public string ChannelName { get; set; }
        public string RegisterEmployee { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string RegisterDates { get; set; }
        public DateTime? WeddingDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string WeddingDates { get; set; }
        public string PointName { get; set; }
        public string DateCreate { get; set; }
        public Double Point { get; set; }
        public int YOB { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public DateTime? DOB { get; set; }
        public string DOBs { get; set; }
        public int? StoreId { get; set; }
        public int? ChannelId { get; set; }
        public int? BrandId { get; set; }
        public int? ProductId { get; set; }
        public int  num { get; set; }
        public string address1 { get; set; }
        public string FullAddress { get; set; }
        public string[] Address { get; set; }
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
        public bool TermsOfUse { get; set; }
        public string ReferralMember { get; set; }
        public string ProvinceName { get; set; }
        public int? PreferredProduct { get; set; }
        public Guid? PreferredBrand { get; set; }
        public int? JobTitleId { get; set; }
        public string JobTitle { get; set; }
        public int? OccupationId { get; set; }
        public string Occupation { get; set; }
        public bool valid { get; set; }
        public string ImageUrl { get; set; }

        public MARITAL_STATUS? MaritalStatus { get; set; }
        public bool AcceptToReceiveMarketing { get; set; }
        public bool AcceptToReceiveLoyaltyInformation { get; set; }
        public bool AcceptToBeContactedViaMobilePhone { get; set; }
        public bool AcceptToBeContactedViaMobileEmail { get; set; }
        public bool NotAcceptAnyContact { get; set; }


        public IEnumerable<SelectListItem> Year { get; set; }

    }
}
