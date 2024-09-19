using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Coupon;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface ICouponCategoryService
    {
        List<CouponCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        CouponCategoryViewModel GetCouponCategory(int id);
        void CreateCouponCategory(CouponCategoryViewModel model);
        void UpdateCouponCategory(CouponCategoryViewModel model);
        void DeleteCouponCategory(int id);
        List<CouponCategoryViewModel> GetCouponCategorys();
    }
}
