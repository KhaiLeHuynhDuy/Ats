using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Reward
{
    public class RedeemPointDetailViewModel
    {
        public Guid MemberId { get; set; }
        public string Email { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TierName { get; set; }       
        public string PhoneNumber { get; set; }
        public double AvailablePoints { get; set; }
        public double RedeemedRate { get; set; }
        [Required]
        public double? PointRedemption { get; set; }
        [Required]
        public string VerificationCode { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        public string Remark { get; set; }        
    }
}
