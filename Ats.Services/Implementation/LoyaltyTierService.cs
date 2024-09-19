using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Loyalty.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.LoyaltyTier;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class LoyaltyTierService : BaseService, ILoyaltyTierService
    {
        private IConfiguration _config;
        private int pageSize;

        public LoyaltyTierService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<LoyaltyTierViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.LoyaltyTierRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.TierName != null && x.TierName.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.TierName) : query.OrderByDescending(x => x.TierName);
                        break;
                    case "level":
                        query = IsAscOrder ? query.OrderBy(x => x.TierLevel) : query.OrderByDescending(x => x.TierLevel);
                        break;
                    case "upgrade":
                        query = IsAscOrder ? query.OrderBy(x => x.UpgradePoint) : query.OrderByDescending(x => x.UpgradePoint);
                        break;
                    case "downgrade":
                        query = IsAscOrder ? query.OrderBy(x => x.DowngradePoint) : query.OrderByDescending(x => x.DowngradePoint);
                        break;
                    case "min":
                        query = IsAscOrder ? query.OrderBy(x => x.PointMin) : query.OrderByDescending(x => x.PointMin);
                        break;
                    case "max":
                        query = IsAscOrder ? query.OrderBy(x => x.PointMax) : query.OrderByDescending(x => x.PointMax);
                        break;
                  
                }
            }

            var data = query.Select(x => new LoyaltyTierViewModel()
            {
                Id = x.Id,
                TierName = x.TierName,
                TierLevel = x.TierLevel,
                UpgradePoint = x.UpgradePoint,
                DowngradePoint = x.DowngradePoint,
                PointMin = x.PointMin,
                PointMax = x.PointMax,
                Desc = x.Desc,
                Active = x.Active
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<LoyaltyTierViewModel> GetLoyaltyTiers()
        {
            _logger.LogDebug($"Get LoyaltyTier all");
            var loyaltyTiers = UnitOfWork.LoyaltyTierRepo.GetAll().Where(x => x.Active).Select(x => new LoyaltyTierViewModel()
            {
                Id = x.Id,
                TierName = x.TierName,
                TierLevel = x.TierLevel,
                UpgradePoint = x.UpgradePoint,
                DowngradePoint = x.DowngradePoint,
                PointMin = x.PointMin,
                PointMax = x.PointMax,             
                Desc = x.Desc,             
                Active = x.Active,             
            }).OrderBy(x => x.TierName).ToList();

            return loyaltyTiers;
        }

        public LoyaltyTierViewModel GetLoyaltyTier(int id)
        {
            _logger.LogDebug($"LoyaltyTiers Detail service (Id: {id})");
            var entity = UnitOfWork.LoyaltyTierRepo.GetById(id);
            if (entity != null)
            {
                var model = new LoyaltyTierViewModel()
                {
                    Id = entity.Id,
                    TierName = entity.TierName,
                    TierLevel = entity.TierLevel,
                    UpgradePoint = entity.UpgradePoint,
                    DowngradePoint = entity.DowngradePoint,
                    PointMin = entity.PointMin,
                    PointMax = entity.PointMax,
                    Desc = entity.Desc,
                    Active = entity.Active
                };

                return model;
            }
            return null;
        }
        public void CreateLoyaltyTier(LoyaltyTierViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyTier (Name: {model.TierName})");
            var entity = new LoyaltyTier
            {
                Id = model.Id,
                TierName = model.TierName,
                TierLevel = model.TierLevel,
                UpgradePoint = model.UpgradePoint,
                DowngradePoint = model.DowngradePoint,
                PointMin = model.PointMin,
                PointMax = model.PointMax,
                Desc = model.Desc,
                Active = model.Active
            };

            UnitOfWork.LoyaltyTierRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyTier(LoyaltyTierViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyTier (Id: {model.Id})");
            var entity = UnitOfWork.LoyaltyTierRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.TierName = model.TierName;
                entity.TierLevel = model.TierLevel;
                entity.UpgradePoint = model.UpgradePoint;
                entity.DowngradePoint = model.DowngradePoint;
                entity.PointMax = model.PointMax;
                entity.PointMin = model.PointMin;
                entity.Desc = model.Desc;
                entity.Active = model.Active;
                UnitOfWork.LoyaltyTierRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyTier(int id)
        {
            _logger.LogDebug($"Delete LoyaltyTier (Id: {id})");
            var entity = UnitOfWork.LoyaltyTierRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoyaltyTierRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

       
    }
}
