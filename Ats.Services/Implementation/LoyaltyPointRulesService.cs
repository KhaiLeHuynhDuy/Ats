using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Store;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Models.MemberWallet;
using Ats.Domain.Member.Models;
using Ats.Domain.Loyalty.Models;
using Ats.Domain.Coupon.Models;
using Ats.Commons;
using Ats.Models.Loyalty;
using Ats.Models.Channel;
using Ats.Domain.Channel.Models;
using Ats.Models.Brand;

namespace Ats.Services.Implementation
{
    public class LoyaltyPointRulesService : BaseService, ILoyaltyPointRulesService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public LoyaltyPointRulesService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

        #region List category
        public List<CouponCategory> GetCouponCategories()
        {
            _logger.LogDebug($"Get Coupon Categories Enter.");
            var couponCategories = UnitOfWork.CouponCategoryRepo.GetAll().Where(x => x.Active).Select(x => new CouponCategory()
            {
                Id = x.Id,
                CouponName = x.CouponName
            }).OrderBy(x => x.CouponName).ToList();

            return couponCategories;
        }
        #endregion


        public List<LoyaltyPointTypeViewModel> SearchLoyaltyPointType(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            var query = UnitOfWork.LoyaltyPointTypeRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;

                    case "RuleName":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<LoyaltyPointTypeViewModel> data = new List<LoyaltyPointTypeViewModel>();
            foreach (var item in datas)
            {
                LoyaltyPointTypeViewModel pointRule = new LoyaltyPointTypeViewModel();


                pointRule.Id = item.Id;
                pointRule.Name = item.Name;
                pointRule.StandardType = item.StandardType;
                pointRule.Decs = item.Decs;
                pointRule.Active = item.Active;
              
                data.Add(pointRule);
            }

            return data;
        }



        #region Loyalty Point Rule
        public List<LoyaltyPointRulesViewModel> SearchLoyaltyPointRule(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            var query = UnitOfWork.LoyaltyPointRuleRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.RuleName != null && x.RuleName.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;

                    case "rulename":
                        query = IsAscOrder ? query.OrderBy(x => x.RuleName) : query.OrderByDescending(x => x.RuleName);
                        break;
                    case "type":
                        query = IsAscOrder ? query.OrderBy(x => x.BonusType) : query.OrderByDescending(x => x.BonusType);
                        break;
                    case "startfrom":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveFrom) : query.OrderByDescending(x => x.EffectiveFrom);
                        break;
                    case "endto":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveTo) : query.OrderByDescending(x => x.EffectiveTo);
                        break;
                    case "pointredemptionrule":
                        query = IsAscOrder ? query.OrderBy(x => x.RedemptionRate) : query.OrderByDescending(x => x.RedemptionRate);
                        break;
                    case "earningrate":
                        query = IsAscOrder ? query.OrderBy(x => x.EarningRate) : query.OrderByDescending(x => x.EarningRate);
                        break;
                    
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<LoyaltyPointRulesViewModel> data = new List<LoyaltyPointRulesViewModel>();
            foreach (var item in datas)
            {
                LoyaltyPointRulesViewModel pointRule = new LoyaltyPointRulesViewModel();


                pointRule.Id = item.Id;
                pointRule.RuleName = item.RuleName;
                pointRule.EffectiveFroms = string.Format("{0:dd/MM/yyyy}", item.EffectiveFrom);
                pointRule.EffectiveTos = string.Format("{0:dd/MM/yyyy}", item.EffectiveTo);
                pointRule.Valid = item.Valid;
                pointRule.BonusType = item.BonusType;
                pointRule.RedemptionRate = item.RedemptionRate;
                pointRule.EarningRate = item.EarningRate;
                pointRule.Remark = item.Remark;
                data.Add(pointRule);
            }

            return data;
        }

        public List<LoyaltyPointRulesViewModel> GetLoyaltyPointRules()
        {
            _logger.LogDebug($"Get all LoyaltyPointType ");
            var loyaltyPointTypes = UnitOfWork.LoyaltyPointRuleRepo.GetAll().Select(x => new LoyaltyPointRulesViewModel()
            {
                Id = x.Id,
                RuleName = x.RuleName,
                RedemptionRate = x.RedemptionRate,
                EarningRate = x.EarningRate,
                Remark = x.Remark,            
            }).OrderBy(x => x.RuleName).ToList();

            return loyaltyPointTypes;
        }

        public LoyaltyPointRulesViewModel GetLoyaltyPointRule(Guid id)
        {
            _logger.LogDebug($" Custom Point rules Detail service (Id: {id})");
            var entity = UnitOfWork.LoyaltyPointRuleRepo.GetById(id);
            if (entity != null)
            {
                var model = new LoyaltyPointRulesViewModel()
                {
                    Id = entity.Id,
                    RuleName = entity.RuleName,
                    RedemptionRate = entity.RedemptionRate,
                    EarningRate = entity.EarningRate,
                    Remark = entity.Remark,
                    Valid = entity.Valid,
                    BonusType = entity.BonusType,
                    EffectiveFrom = entity.EffectiveFrom,
                    EffectiveTo = entity.EffectiveTo,
                };
                //PointRuleStore
                foreach (PointRuleStore itemStore in entity.PointRuleStores.OrderBy(x=> x.Store.StoreName).ToList() )
                {
                    var lsStore = new PointRuleStoresViewModel()
                    {
                        Id = itemStore.Id,
                        StoreId = itemStore.StoreId,
                        StoreName= itemStore.Store.StoreName,
                        Address= itemStore.Store.Address,
                        City=itemStore.Store.City,
                        LoyaltyPointRuleId = itemStore.LoyaltyPointRuleId,
                        Valid = itemStore.Valid,
                       
                    };
                    model.PointRuleStores.Add(lsStore);
                }
                //PointRuleChannel
                foreach (PointRuleChannel itemChannel in entity.PointRuleChannels.OrderBy(x => x.Channel.ChannelName).ToList())
                {
                    var lsChannel = new PointRuleChannelViewModel()
                    {
                        Id = itemChannel.Id,
                        ChannelId = itemChannel.ChannelId,
                        ChannelName = itemChannel.Channel.ChannelName,
                       
                        Desc = itemChannel.Channel.Desc,
                        LoyaltyPointRuleId = itemChannel.LoyaltyPointRuleId,
                        Valid = itemChannel.Valid,

                    };
                    model.PointRuleChannels.Add(lsChannel);
                }
                //PointRuleProduct
                foreach (PointRuleProduct itemProduct in entity.PointRuleProducts.OrderBy(x => x.Product.Name).ToList())
                {
                    var lsProduct = new PointRuleProductViewModel()
                    {
                        Id = itemProduct.Id,
                        ProductId = itemProduct.ProductId,
                        ProductName = itemProduct.Product.Name,
                        Code= itemProduct.Product.Code,
                        Desc= itemProduct.Product.Description,
                        LoyaltyPointRuleId = itemProduct.LoyaltyPointRuleId,
                        Valid = itemProduct.Valid,
                       
                    };
                    model.PointRuleProducts.Add(lsProduct);
                }
                //PointRuleBrand
                //foreach (PointRuleBrand itemBrand in entity.PointRuleBrands.OrderBy(x => x.Brand.BrandName).ToList())
                //{
                //    var lsBrand = new PointRuleBrandViewModel()
                //    {
                //        Id = itemBrand.Id,
                //        BrandId = itemBrand.BrandId,
                //        BrandName = itemBrand.Brand.BrandName,
                //        BrandCode = itemBrand.Brand.BrandCode,
                //        Desc = itemBrand.Brand.Desc,
                //        LoyaltyPointRuleId = itemBrand.LoyaltyPointRuleId,
                //        Valid = itemBrand.Valid,
                       
                //    };
                //    model.pointRuleBrands.Add(lsBrand);
                //}
                //PointRuleCategory
                foreach (PointRuleCategory itemCate in entity.PointRuleCategories.OrderBy(x => x.ProductCategory.Name).ToList())
                {
                    var lsCate = new PointRuleCategoryViewModel()
                    {
                        Id = itemCate.Id,
                        ProductCateId = itemCate.ProductCateId,
                        Name= itemCate.ProductCategory.Name,
                        DisplayOrder=itemCate.ProductCategory.DisplayOrder,
                        Description=itemCate.ProductCategory.Description,
                        LoyaltyPointRuleId = itemCate.LoyaltyPointRuleId,
                        Valid = itemCate.Valid,

                    };
                    model.PointRuleCategories.Add(lsCate);
                }
                return model;
            }
            return null;
        }

        public void CreateLoyaltyPointRule(LoyaltyPointRulesViewModel model)
        {
            _logger.LogDebug($"Create Custom Point rules (Name: {model.RuleName})");
            var entity = new LoyaltyPointRule
            {
                Id = model.Id,
                RuleName = model.RuleName,
                RedemptionRate = model.RedemptionRate,
                BonusType = model.BonusType,
                EarningRate = model.EarningRate,
                Valid = model.Valid,
                Remark = model.Remark,

                EffectiveFrom = model.EffectiveFrom,
                EffectiveTo = model.EffectiveTo,
            };

            UnitOfWork.LoyaltyPointRuleRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateLoyaltyPointRule(LoyaltyPointRulesViewModel model)
        {
            _logger.LogDebug($"Edit Custom Point rules (Id: {model.Id})");
            var entity = UnitOfWork.LoyaltyPointRuleRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.RuleName = model.RuleName;
                entity.RedemptionRate = model.RedemptionRate;
                entity.EarningRate = model.EarningRate;
                entity.BonusType = model.BonusType;
                entity.Remark = model.Remark;
                entity.Valid = model.Valid;

                entity.EffectiveFrom = model.EffectiveFrom;
                entity.EffectiveTo = model.EffectiveTo;

                UnitOfWork.LoyaltyPointRuleRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteLoyaltyPointRule(Guid id)
        {
            _logger.LogDebug($"Delete Custom Point rules (Id: {id})");
            var entity = UnitOfWork.LoyaltyPointRuleRepo.GetById(id);
            if (entity != null)
            {
         

                UnitOfWork.LoyaltyPointRuleRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        #endregion Loyalty Point Rule

        #region Item Store
      
        public void CreateLoyaltyItemPointRuleStore(PointRuleStoresViewModel model)
        {
            _logger.LogDebug($"Create Custom Point rules Store(Name: {model.Id})");

            PointRuleStore entity = new PointRuleStore();
            entity.Id = Guid.NewGuid();

            entity.StoreId = model.StoreId;
            entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;

            entity.EarningRate = model.EarningRate;
            entity.PointRedemption = model.PointRedemption;
            entity.Remark = model.Remark;
            entity.Valid = model.Valid;

            entity.EffectiveTo = model.EffectiveTo;
            entity.EffectiveFrom = model.EffectiveFrom;

            UnitOfWork.PointRuleStoresRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyItemPointRuleStore(PointRuleStoresViewModel model)
        {
            _logger.LogDebug($"Edit Custom Point rules Store (Id: {model.Id})");
            var entity = UnitOfWork.PointRuleStoresRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Id = model.Id;
                entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;
                entity.EarningRate = model.EarningRate;
                entity.PointRedemption = model.PointRedemption;
                entity.Remark = model.Remark;
                entity.Valid = model.Valid;
                entity.EffectiveFrom = model.EffectiveFrom;
                entity.EffectiveTo = model.EffectiveTo;

                UnitOfWork.PointRuleStoresRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyItemPointRuleStore(Guid id)
        {
            _logger.LogDebug($"Delete Custom Point rules Store (Id: {id})");
            var entity = UnitOfWork.PointRuleStoresRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.PointRuleStoresRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public PointRuleStoresViewModel GetPointRuleStore(Guid id)
        {
            _logger.LogDebug($"Get Point Rule Stores detail (Id: {id})");
            var entity = UnitOfWork.PointRuleStoresRepo.GetById(id);
            if (entity != null)
            {
                PointRuleStoresViewModel model = new PointRuleStoresViewModel()
                {
                    Id = entity.Id,
                    LoyaltyPointRuleId = entity.LoyaltyPointRuleId,
                    StoreId = entity.StoreId,
                    Valid = entity.Valid,

                };
                return model;
            }
            return null;
        }

        #endregion

        #region Item Product

        public void CreateLoyaltyItemPointRuleProduct(PointRuleProductViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyPointRule Product(Name: {model.Id})");

            PointRuleProduct entity = new PointRuleProduct();
            entity.Id = Guid.NewGuid();

            entity.ProductId = model.ProductId;
            entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;

            entity.EarningRate = model.EarningRate;
            entity.PointRedemption = model.PointRedemption;
            entity.Remark = model.Remark;
            entity.Valid = true;

            entity.EffectiveTo = model.EffectiveTo;
            entity.EffectiveFrom = model.EffectiveFrom;

            UnitOfWork.PointRuleProductRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyItemPointRuleProduct(PointRuleProductViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyPointRule Store (Id: {model.Id})");
            var entity = UnitOfWork.PointRuleProductRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Id = model.Id;
                entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;

                entity.EarningRate = model.EarningRate;
                entity.PointRedemption = model.PointRedemption;
                entity.Remark = model.Remark;
                entity.Valid = model.Valid;

                entity.EffectiveFrom = model.EffectiveFrom;
                entity.EffectiveTo = model.EffectiveTo;

                UnitOfWork.PointRuleProductRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyItemPointRuleProduct(Guid id)
        {
            _logger.LogDebug($"Delete LoyaltyPointRule Product (Id: {id})");
            var entity = UnitOfWork.PointRuleProductRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.PointRuleProductRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public PointRuleProductViewModel GetPointRuleProduct(Guid id)
        {
            _logger.LogDebug($"Get Point Rule Products detail (Id: {id})");
            var entity = UnitOfWork.PointRuleProductRepo.GetById(id);
            if (entity != null)
            {
                PointRuleProductViewModel model = new PointRuleProductViewModel()
                {
                    Id = entity.Id,
                    LoyaltyPointRuleId = entity.LoyaltyPointRuleId,
                    ProductId = entity.ProductId,
                    Valid = entity.Valid,

                };
                return model;
            }
            return null;
        }
        #endregion

        #region Item Channel

        public void CreateLoyaltyItemPointRuleChannel(PointRuleChannelViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyPointRule (Name: {model.Id})");
            var entity = new PointRuleChannel
            {
                Id = model.Id,
                ChannelId = model.ChannelId,
                LoyaltyPointRuleId = model.LoyaltyPointRuleId,
                EarningRate = model.EarningRate,
                PointRedemption = model.PointRedemption,
                Valid = model.Valid,
                Remark = model.Remark,

                EffectiveFrom = model.EffectiveFrom,
                EffectiveTo = model.EffectiveTo,
            };

            UnitOfWork.PointRuleChannelRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyItemPointRuleChannel(PointRuleChannelViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyPointRule (Id: {model.Id})");
            var entity = UnitOfWork.PointRuleChannelRepo.GetById(model.Id);
            if (entity != null)
            {
                //entity.ChannelId = model.ChannelId;
                //entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;
                entity.EarningRate = model.EarningRate;
                entity.PointRedemption = model.PointRedemption;
                entity.Remark = model.Remark;
                entity.Valid = model.Valid;

                entity.EffectiveFrom = model.EffectiveFrom;
                entity.EffectiveTo = model.EffectiveTo;

                UnitOfWork.PointRuleChannelRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyItemPointRuleChannel(Guid id)
        {
            _logger.LogDebug($"Delete LoyaltyPointType (Id: {id})");
            var entity = UnitOfWork.PointRuleChannelRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.PointRuleChannelRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public PointRuleChannelViewModel GetPointRuleChannel(Guid id)
        {
            _logger.LogDebug($"Get Point Rule Channels detail (Id: {id})");
            var entity = UnitOfWork.PointRuleChannelRepo.GetById(id);
            if (entity != null)
            {
                PointRuleChannelViewModel model = new PointRuleChannelViewModel()
                {
                    Id = entity.Id,
                    LoyaltyPointRuleId = entity.LoyaltyPointRuleId,
                    ChannelId = entity.ChannelId,
                    Valid = entity.Valid,

                };
                return model;
            }
            return null;
        }
        #endregion

        #region Item Brand

        public void CreateLoyaltyItemPointRuleBrand(PointRuleBrandViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyPointRule (Name: {model.Id})");
            var entity = new PointRuleBrand
            {
                Id = model.Id,
                //BrandId = model.BrandId,
                LoyaltyPointRuleId = model.LoyaltyPointRuleId,
                EarningRate = model.EarningRate,
                PointRedemption = model.PointRedemption,
                Valid = model.Valid,
                Remark = model.Remark,

                EffectiveFrom = model.EffectiveFrom,
                EffectiveTo = model.EffectiveTo,
            };

            UnitOfWork.PointRuleBrandRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyItemPointRuleBrand(PointRuleBrandViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyPointRule (Id: {model.Id})");
            var entity = UnitOfWork.PointRuleBrandRepo.GetById(model.Id);
            if (entity != null)
            {
                //entity.BrandId = model.BrandId;
                //entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;
                entity.EarningRate = model.EarningRate;
                entity.PointRedemption = model.PointRedemption;
                entity.Remark = model.Remark;
                entity.Valid = model.Valid;

                entity.EffectiveFrom = model.EffectiveFrom;
                entity.EffectiveTo = model.EffectiveTo;

                UnitOfWork.PointRuleBrandRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyItemPointRuleBrand(Guid id)
        {
            _logger.LogDebug($"Delete LoyaltyPointType (Id: {id})");
            var entity = UnitOfWork.PointRuleBrandRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.PointRuleBrandRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public PointRuleBrandViewModel GetPointRuleBrand(Guid id)
        {
            _logger.LogDebug($"Get Point Rule Brands detail (Id: {id})");
            var entity = UnitOfWork.PointRuleBrandRepo.GetById(id);
            if (entity != null)
            {
                PointRuleBrandViewModel model = new PointRuleBrandViewModel()
                {
                    Id = entity.Id,
                    LoyaltyPointRuleId = entity.LoyaltyPointRuleId,
                    //BrandId = entity.BrandId,
                    Valid = entity.Valid,

                };
                return model;
            }
            return null;
        }

        #endregion

        #region Item Category

        public void CreateLoyaltyItemPointRuleCategory(PointRuleCategoryViewModel model)
        {
            _logger.LogDebug($"Create Custom Point rules (Name: {model.Id})");
            var entity = new PointRuleCategory
            {
                Id = model.Id,
                ProductCateId = model.ProductCateId,
                LoyaltyPointRuleId = model.LoyaltyPointRuleId,               
                Valid = model.Valid,
              
            };

            UnitOfWork.PointRuleCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyItemPointRuleCategory(PointRuleCategoryViewModel model)
        {
            _logger.LogDebug($"Edit Custom Point rules (Id: {model.Id})");
            var entity = UnitOfWork.PointRuleCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                //entity.ProductCateId = model.ProductCateId;
                //entity.LoyaltyPointRuleId = model.LoyaltyPointRuleId;              
                entity.Valid = model.Valid;

                UnitOfWork.PointRuleCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyItemPointRuleCategory(Guid id)
        {
            _logger.LogDebug($"Delete Custom Point rules (Id: {id})");
            var entity = UnitOfWork.PointRuleCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.PointRuleCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public PointRuleCategoryViewModel GetPointRuleCate(Guid id)
        {
            _logger.LogDebug($"Get Point Rule Categories detail (Id: {id})");
            var entity = UnitOfWork.PointRuleCategoryRepo.GetById(id);
            if (entity != null)
            {
                PointRuleCategoryViewModel model = new PointRuleCategoryViewModel()
                {
                    Id = entity.Id,
                    LoyaltyPointRuleId = entity.LoyaltyPointRuleId,
                    ProductCateId = entity.ProductCateId,
                    Valid = entity.Valid,

                };
                return model;
            }
            return null;
        }

        #endregion

        public bool CheckExistAddStores(Guid? ruleId, int storeId)
        {
            _logger.LogDebug($"Check existed Stores");
            return UnitOfWork.PointRuleStoresRepo.CheckExistAddStores(ruleId, storeId);
        }
        public bool CheckExistAddChannels(Guid? ruleId, int? channelId)
        {
            _logger.LogDebug($"Check existed Channels");
            return UnitOfWork.PointRuleChannelRepo.CheckExistAddChannels(ruleId, channelId);
        }

        public bool CheckExistAddBrands(Guid? ruleId, int? brandId)
        {
            _logger.LogDebug($"Check existed Brands");
            return UnitOfWork.PointRuleBrandRepo.CheckExistAddBrands(ruleId, brandId);
        }

        public bool CheckExistAddCategories(Guid? ruleId, int? cateId)
        {
            _logger.LogDebug($"Check existed Categories");
            return UnitOfWork.PointRuleCategoryRepo.CheckExistAddCategories(ruleId, cateId);
        }

        public bool CheckExistAddProducts(Guid? ruleId, int? productId)
        {
            _logger.LogDebug($"Check existed Products");
            return UnitOfWork.PointRuleProductRepo.CheckExistAddProducts(ruleId, productId);
        }

    }
}
