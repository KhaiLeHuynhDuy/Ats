using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain;
using Ats.Domain.Channel.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Coupon;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface ICouponService
    {
        List<CouponViewModel> Search(string searchText, string orderField, bool IsAscOrder,bool? couponType,int?channelId, string? fromEffectiveDateBegin,
            string? toEffectiveDateBegin, string? fromEffectiveDateEnd, string? toEffectiveDateEnd, EXPIRY_COUPON? expirycoupon, int page, int size, out int total);
        CouponViewModel Get(Guid id);
        void Create(CouponViewModel model);
        void Update(CouponViewModel model);
        void Delete(Guid id);
        List<CouponViewModel> Get();

        #region List Category
        List<CouponCategory> GetCouponCategories();
        List<Coupon> GetCoupon();

        #endregion List Category
    }
}
