using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Channel.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Loyalty.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Brand;
using Ats.Models.Channel;
using Ats.Models.Loyalty;
using Ats.Models.LoyaltyTier;
using Ats.Models.MemberWallet;
using Ats.Models.Product;
using Ats.Models.Store;

namespace Ats.Services.Interfaces
{
    public interface ILoyaltyPointRulesService
    {

        #region List Categories
        List<CouponCategory> GetCouponCategories();
        //List<Store> GetStores();
        #endregion Loyalty
        List<LoyaltyPointTypeViewModel> SearchLoyaltyPointType(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);

        #region Loyalty Point Rules
        List<LoyaltyPointRulesViewModel> SearchLoyaltyPointRule(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        LoyaltyPointRulesViewModel GetLoyaltyPointRule(Guid id);
        void CreateLoyaltyPointRule(LoyaltyPointRulesViewModel model);
        void UpdateLoyaltyPointRule(LoyaltyPointRulesViewModel model);
        void DeleteLoyaltyPointRule(Guid id);
        List<LoyaltyPointRulesViewModel> GetLoyaltyPointRules();
        #endregion Loyalty Point Rules

        #region List Store, Channel, Brand, Product, ProductCategory
        bool CheckExistAddStores(Guid? ruleId, int storeId);
        void CreateLoyaltyItemPointRuleStore(PointRuleStoresViewModel model);
        void UpdateLoyaltyItemPointRuleStore(PointRuleStoresViewModel model);
        void DeleteLoyaltyItemPointRuleStore(Guid id);
        PointRuleStoresViewModel GetPointRuleStore(Guid id);


        bool CheckExistAddProducts(Guid? ruleId, int? productId);
        void CreateLoyaltyItemPointRuleProduct(PointRuleProductViewModel model);
        void UpdateLoyaltyItemPointRuleProduct(PointRuleProductViewModel model);
        void DeleteLoyaltyItemPointRuleProduct(Guid id);
        PointRuleProductViewModel GetPointRuleProduct(Guid id);


        bool CheckExistAddChannels(Guid? ruleId, int? channelId);
        void CreateLoyaltyItemPointRuleChannel(PointRuleChannelViewModel model);
        void UpdateLoyaltyItemPointRuleChannel(PointRuleChannelViewModel model);
        void DeleteLoyaltyItemPointRuleChannel(Guid id);
        PointRuleChannelViewModel GetPointRuleChannel(Guid id);


        bool CheckExistAddBrands(Guid? ruleId, int? brandId);
        void CreateLoyaltyItemPointRuleBrand(PointRuleBrandViewModel model);
        void UpdateLoyaltyItemPointRuleBrand(PointRuleBrandViewModel model);
        void DeleteLoyaltyItemPointRuleBrand(Guid id);
        PointRuleBrandViewModel GetPointRuleBrand(Guid id);


        bool CheckExistAddCategories(Guid? ruleId, int? cateId);
        void CreateLoyaltyItemPointRuleCategory(PointRuleCategoryViewModel model);
        void UpdateLoyaltyItemPointRuleCategory(PointRuleCategoryViewModel model);
        void DeleteLoyaltyItemPointRuleCategory(Guid id);
        PointRuleCategoryViewModel GetPointRuleCate(Guid id);

        #endregion

    }
}
