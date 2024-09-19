using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Brand;
using Ats.Models.Member;
using Ats.Models.Product;
using Ats.Models.Reward;

namespace Ats.Services.Interfaces
{
    public interface IRewardsService
    {
        RedeemPointDetailViewModel GetRedeemPointDetail(Guid memberId);
        void CreateRedeemPoint(RedeemPointDetailViewModel model);
        double GetRedeemedDiscount(double point);
        List<RedemptionItemViewModel> GetListRedemption(SearchInfoRedemptionViewModel searchInfo, out int total);
        RedemptionItemViewModel GetRedemptionDetail(Guid id, GIFT_VOUCHER_COUPON type);
        void RedemptionRedeem(RedemptionItemViewModel model);
    }
}
