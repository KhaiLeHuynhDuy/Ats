using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ats.Domain.Account.Models;
using Ats.Domain.Address;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using static Ats.Commons.Constants;
using Ats.Domain.Store.Models;
using Ats.Domain.Loyalty.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Voucher.Models;
using Ats.Domain.Gift.Models;

namespace Ats.Domain.Member.Models
{
    [Table("members")]
    public class Member : AuditBase, IEntity<Guid>
    {
        public Member()
        {
            MemberWallets = new HashSet<MemberWallet>();
            PreferredBrands = new HashSet<PreferredBrand>();
            PreferredProducts = new HashSet<PreferredProduct>();
            MemberLoyaltyTiers = new HashSet<MemberLoyaltyTier>();
        }
      
        [Key]
        public Guid Id { get; set; }
        public string MemberCode { get; set; }
 
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int YOB { get; set; }  
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        
        public string PhotoUrl { get; set; }

        public Guid? HomeAddress { get; set; }
        [ForeignKey("HomeAddress")]
        public virtual Address.Models.Address Address { get; set; }
        public MARITAL_STATUS? MaritalStatus { get; set; }
        public DateTime? WeddingDate { get; set; }     
        public bool AcceptToReceiveMarketing { get; set; }
        public bool AcceptToReceiveLoyaltyInformation { get; set; }
        public bool AcceptToBeContactedViaMobilePhone { get; set; }
        public bool AcceptToBeContactedViaMobileEmail { get; set; }
        public bool NotAcceptAnyContact { get; set; }

        public int? JobTitleId { get; set; }
        [ForeignKey("JobTitleId")]
        public virtual JobTitle JobTitle { get; set; }

        public int? OccupationId { get; set; }
        [ForeignKey("OccupationId")]
        public virtual Occupation Occupation { get; set; }
        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Ats.Domain.Channel.Models.MemberChannel Channel  { get; set; }
        public int? StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Store.Models.Store Store { get; set; }

        public string ZaloId { get; set; }

        public DateTime? RegisterDate { get; set; }
        public Guid? RegisterEmployee { get; set; }
        [ForeignKey("RegisterEmployee")]
        public virtual User User { get; set; }

        public virtual ICollection<MemberWallet> MemberWallets { get; set; }
        public virtual ICollection<PreferredBrand> PreferredBrands { get; set; }

        public virtual ICollection<PreferredProduct> PreferredProducts { get; set; }
        public virtual ICollection<MemberLoyaltyTier> MemberLoyaltyTiers { get; set; }

        public virtual List<MemberCoupon> MemberCoupons { get; set; }

        public virtual List<MemberVoucher> MemberVouchers { get; set; }

        public virtual List<MemberGift> MemberGifts { get; set; }

        //public virtual ICollection<MemberTag> MemberTags { get; set; }
        public virtual List<MemberTagging> Tags { get; set; }

        //public virtual ICollection<MemberGroup> MemberGroups { get; set; }
        public virtual List<MemberGroupLink> MemberGroupLinks { get; set; }

        //public virtual ICollection<MemberSegment> MemberSegments { get; set; }
        public virtual List<MemberSegmentLink> MemberSegmentLinks { get; set; }

        //public virtual ICollection<MemberLifeCycle> MemberLifeCycles { get; set; }
        public virtual List<MemberLifeCycleLink> MemberLifeCycleLinks { get; set; }
        public virtual List<PurchasedTransaction> PurchasedTransactions { get; set; }        
    }
}





   
