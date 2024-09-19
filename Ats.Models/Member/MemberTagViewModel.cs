using Ats.Domain;
using Ats.Domain.Lead.Models;
using Ats.Domain.Member.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberTagViewModel
    {
   
        public Guid Id { get; set; }
        public int TagCagetoryId { get; set; }
        public string TagCategoryName { get; set; }
        public bool TagType { get; set; }
        public string TagName { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUpdates { get; set; }
        public int TotalMember { get; set; }

        public TAG_KEY TAGKEYs { get; set; }

        public bool GenderSelected { get; set; }
        public bool GenderMale { get; set; }
        public bool GenderFemale { get; set; }


        public bool AgeSelected { get; set; }
        public string AgeFrom { get; set; }
        public string AgeTo { get; set; }


        public bool BirthdaySelected { get; set; }
        public string BirthdayValue { get; set; }

        public bool BirthdayMonthSelected { get; set; }
        public string[] BirthdayMonthValue { get; set; }
        public IEnumerable<SelectListItem> BirthdayMonthList { get; set; }


        public bool ProvinceSelected { get; set; }
        public string[] ProvinceValue { get; set; }
        public IEnumerable<SelectListItem> ProvinceList { get; set; }

        public bool WeddingAnniversarySelected { get; set; }
        public string WeddingAnniValue { get; set; }


        public bool MarriedYearSelected { get; set; }
        public string[] MarriedYearValue { get; set; }
        public IEnumerable<SelectListItem> MarriedYearList { get; set; }

        public bool WeddingMonthSelected { get; set; }
        public string[] WeddingMonthValue { get; set; }
        public IEnumerable<SelectListItem> WeddingMonthList { get; set; }

        public bool MarriedStatusSelected { get; set; }
        public string[] MarriedStatusValue { get; set; }
        public IEnumerable<SelectListItem> MarriedStatusList { get; set; }

        public bool PreferredBrandSelected { get; set; }
        public string[] PreferredBrandValue { get; set; }
        public IEnumerable<SelectListItem> PreferredBrandList { get; set; }

        public bool PreferredStoreSelected { get; set; }

        public string[] PreferredStoreValue { get; set; }
        public IEnumerable<SelectListItem> PreferredStoreList { get; set; }



        public bool PreferredProductSelected { get; set; }
        public string[] PreferredProductValue { get; set; }
        public IEnumerable<SelectListItem> PreferredProductList { get; set; }

        public bool MemberTierSelected { get; set; }
        public string[] MemberTierValue { get; set; }
        public IEnumerable<SelectListItem> MemberTierList { get; set; }

        public bool RegistrationDateSelected { get; set; }
        public string RegistrationDateValue { get; set; }

        public bool AvailablePointSelected { get; set; }
        public string AvailablePointValue { get; set; }


        public string PointFrom { get; set; }
        public string PointTo { get; set; }
        public bool RegistationStoreSelected { get; set; }
        public string RegistationStoreValue { get; set; }

        public bool RegistationChannelSelected { get; set; }
        public bool RegistationChannelMegapointCloud { get; set; }
        public bool RegistationChannelCorporateWebsite { get; set; }
        public bool RegistationChannelStorePos { get; set; }
        public bool RegistationChannelZalo { get; set; }
        public bool RegistationChannelECommerce { get; set; }
        public bool RegistationChannelMemberPortal { get; set; }

        public bool ReferralRefererSelected { get; set; }
        public string ReferralRefererValue { get; set; }

        public bool ReferralRefereeSelected { get; set; }
        public string ReferralRefereeValue { get; set; }

        public bool ReferralReferNumberSelected { get; set; }
        public string ReferralReferNumberValue { get; set; }

        public bool FirstTransactionDateSelected { get; set; }
        public string FirstTransactionDateValue { get; set; }

        public bool LastTransactionDateSelected { get; set; }
        public string LastTransactionDateValue { get; set; }

        public bool NumberOfTransactionSelected { get; set; }
        public string NumberOfTransactionValue { get; set; }
        public string NumberOfTransactionFrom { get; set; }
        public string NumberOfTransactionTo { get; set; }

        public bool AmountOfTransactionSelected { get; set; }
        public string AmountOfTransactionValue { get; set; }
        public string AmountOfTransactionFrom { get; set; }
        public string AmountOfTransactionTo { get; set; }

        public bool PurchasedBrandSelected { get; set; }
        public bool PurchasedProductSelected { get; set; }

        public bool MemberLifecycleSelected { get; set; }
        public string [] MemberLifecycleValue { get; set; }
        public IEnumerable<SelectListItem> MemberLifecycleList { get; set; }

        public bool MemberLicycleFirstPurchaser { get; set; }
        public bool MemberLicycleLapsePurchaser { get; set; }
        public bool MemberLicycleLoyaltPurchaser { get; set; }
        public bool MemberLicycleNewProspect { get; set; }
        public bool MemberLicycleRepurchaser { get; set; }
        public bool MemberLicycleRepeatPurchaser { get; set; }
        public bool MemberLicycleSpeelpingProspect { get; set; }
        public bool MemberLicycleSpeelpingPurchaser { get; set; }


        public bool CommunicationPreferenceSelected { get; set; }
        public bool CommunicationPreferenceSms{ get; set; }
        public bool CommunicationPreferenceEmail { get; set; }
        public bool CommunicationPreferenceMobile { get; set; }
        public bool CommunicationPreferenceMarketing { get; set; }
        public bool CommunicationPreferenceLoayalty { get; set; }


    
    
    }


  
}
