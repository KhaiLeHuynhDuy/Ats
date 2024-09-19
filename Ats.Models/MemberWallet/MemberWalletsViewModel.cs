using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.MemberWallets
{
   public class MemberWalletsViewModel
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }

        public int? CouponCategoryId { get; set; }
        public double Point { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
        public string Remark { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? CouponId { get; set; }
        public double TrasPoint { get; set; }
        public List<MemberWalletsViewModel> memberWalletsViewModels { get; set; }
        public MemberWalletsViewModel mbWalletsViewModels { get; set; }
        public MemberLoyaltyTransactionsViewModel mbLoyalty { get; set; }
    }
}
