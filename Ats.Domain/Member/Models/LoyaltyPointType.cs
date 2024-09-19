using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    public enum LOYAlTY_POINT_TYPE
    {
        [Display(Name = "Transaction Points")]
        TRANSACTION_POINTS = 1,
        [Display(Name = "Tier Bonus Points")]
        TIER_BONUS_POINTS = 2,
        [Display(Name = "Expired Points")]
        EXPIRED_POINTS = 3, 
        [Display(Name = "Campaign Points")]
        CAMPAIGN_POINTS = 4,
        [Display(Name = "Redemption Points")]
        REDEMPTION_POINTS = 5,
        [Display(Name = "Redemption Rollback Point")]
        REDEMPTION_ROLLBACK_POINT = 6,
        [Display(Name = "Welcome Points")]
        WELCOME_POINTS = 7,
        [Display(Name = "Pay-with Points")]
        PAY_POINTS = 8,
        [Display(Name = "Transferred Points")]
        TRANSFERRED_POINTS = 9,
        [Display(Name = "Sign-in Points")]
        SIGN_POINTS = 10,
        [Display(Name = "Referral Points")]
        REFERRAL_POINTS = 11,   
        [Display(Name = "Adjusted Points")]
        ADJUSTED_POINTS = 12,
        [Display(Name = "Custom Points")]
        CUSTOM_POINTS = 13,
        [Display(Name = "Redemption Points Product")]
        REDEMPTION_POINTS_PRODUCT = 14,
        [Display(Name = "Redemption Rollback Product")]
        REDEMPTION_ROLLBACK_PRODUCT = 15,

    }
    [Table("loyaltypointtypes")]
    public class LoyaltyPointType : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool StandardType { get; set; }
        public bool Active { get; set; }
        public string Decs { get; set; }
    }
}
