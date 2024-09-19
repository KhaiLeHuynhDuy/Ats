using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Coupon;
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
    public class CouponCategoryService : BaseService, ICouponCategoryService
    {
        private IConfiguration _config;
        private int pageSize;

        public CouponCategoryService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<CouponCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.CouponCategoryRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.CouponName != null && x.CouponName.ToLower().Contains(searchText.ToLower()));
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
                        query = IsAscOrder ? query.OrderBy(x => x.CouponName) : query.OrderByDescending(x => x.CouponName);
                        break;
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.CouponCode) : query.OrderByDescending(x => x.CouponCode);
                        break;
                }
            }

            var data = query.Select(x => new CouponCategoryViewModel()
            {
                Id = x.Id,
                CouponCode = x.CouponCode,
                CouponName = x.CouponName,
                Desc = x.Desc,
                Active = x.Active
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<CouponCategoryViewModel> GetCouponCategorys()
        {
            _logger.LogDebug($"Get all CouponCategory ");
            var couponCategorys = UnitOfWork.CouponCategoryRepo.GetAll().Where(x => x.Active).Select(x => new CouponCategoryViewModel()
            {
                Id = x.Id,
                CouponCode = x.CouponCode,
                CouponName = x.CouponName,
                Desc = x.Desc,
            }).OrderBy(x => x.CouponName).ToList();

            return couponCategorys;
        }

        public CouponCategoryViewModel GetCouponCategory(int id)
        {
            _logger.LogDebug($"CouponCategory Detail service (Id: {id})");
            var entity = UnitOfWork.CouponCategoryRepo.GetById(id);
            if (entity != null)
            {
                var model = new CouponCategoryViewModel()
                {
                    Id = entity.Id,
                    CouponCode = entity.CouponCode,
                    CouponName = entity.CouponName,
                    Desc = entity.Desc,
                    Active = entity.Active
                };

                return model;
            }
            return null;
        }
        public void CreateCouponCategory(CouponCategoryViewModel model)
        {
            _logger.LogDebug($"Create CouponCategory (Name: {model.CouponName})");
            var entity = new CouponCategory
            {
                Id = model.Id,
                CouponCode = model.CouponCode,
                CouponName = model.CouponName,
                Desc = model.Desc,
                Active = model.Active
            };

            UnitOfWork.CouponCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateCouponCategory(CouponCategoryViewModel model)
        {
            _logger.LogDebug($"Edit CouponCategory (Id: {model.Id})");
            var entity = UnitOfWork.CouponCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.CouponCode = model.CouponCode;
                entity.CouponName = model.CouponName;
                entity.Desc = model.Desc;
                entity.Active = model.Active;
                UnitOfWork.CouponCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteCouponCategory(int id)
        {
            _logger.LogDebug($"Delete CouponCategory (Id: {id})");
            var entity = UnitOfWork.CouponCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.CouponCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
