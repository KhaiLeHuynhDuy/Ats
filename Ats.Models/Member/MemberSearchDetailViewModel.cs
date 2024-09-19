using Ats.Models.MemberWallet;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
   public class MemberSearchDetailViewModel : BaseSearchViewModel
    {
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public List<MemberWalletHistoryViewModel> MemberWalletsHistorys { get; set; }
        public MemberWalletHistoryViewModel MemberLoyalTran { get; set; }

        public List<GiftVoucherCouponViewModel> GiftVoucherCoupons { get; set; }
        public GiftVoucherCouponViewModel GiftVoucherCoupon { get; set; }

        public List<MemberLoyaltyDetailViewModel> MemberLoyaltyDetails { get; set; }
        public MemberLoyaltyDetailViewModel MemberLoyaltyDetail { get; set; }

        public List<MemberDetailViewModel>  MemberDetailViewModels { get; set; }
        public MemberDetailViewModel MemberDetailViewModel { get; set; }

        public IEnumerable<SelectListItem> Year { get; set; }

       
    }
}
