using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Gift.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Gift;
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
    public class GiftCategoryService : BaseService, IGiftCategoryService
    {
        private IConfiguration _config;
        private int pageSize;

        public GiftCategoryService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<GiftCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            var query = UnitOfWork.GiftCategoryRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.GiftName != null && x.GiftName.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.GiftCode) : query.OrderByDescending(x => x.GiftCode);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.GiftName) : query.OrderByDescending(x => x.GiftName);
                        break;
                }
            }

            var data = query.Select(x => new GiftCategoryViewModel()
            {
                Id = x.Id,
                GiftCode = x.GiftCode,
                GiftName = x.GiftName,
                Active = x.Active,
                Desc = x.Desc
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<GiftCategoryViewModel> Get()
        {
            _logger.LogDebug($"Get all gift category");
            var giftcategory = UnitOfWork.GiftCategoryRepo.GetAll().Where(x => x.Active).Select(x => new GiftCategoryViewModel()
            {
                Id = x.Id,
                GiftCode = x.GiftCode,
                GiftName = x.GiftName,
                Active = x.Active,
                Desc = x.Desc
            }).OrderBy(x => x.GiftName).ToList();

            return giftcategory;
        }

        public GiftCategoryViewModel Get(int id)
        {
            _logger.LogDebug($"Detail giftcategory (Id: {id})");
            var entity = UnitOfWork.GiftCategoryRepo.GetById(id);
            if (entity != null)
            {
                var model = new GiftCategoryViewModel()
                {
                    Id = entity.Id,
                    GiftCode = entity.GiftCode,
                    GiftName = entity.GiftName,
                    Active = entity.Active,
                    Desc = entity.Desc
                };
                return model;
            }
            return null;
        }
        public void Create(GiftCategoryViewModel model)
        {
            _logger.LogDebug($"Create (Name: {model.GiftName})");
            var entity = new GiftCategory
            {
                Id = model.Id,
                GiftCode = model.GiftCode,
                GiftName = model.GiftName,
                Active = model.Active,
                Desc = model.Desc
            };

            UnitOfWork.GiftCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void Update(GiftCategoryViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.GiftCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Id = model.Id;
                entity.GiftCode = model.GiftCode;
                entity.GiftName = model.GiftName;
                entity.Active = model.Active;
                entity.Desc = model.Desc;

                UnitOfWork.GiftCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.GiftCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.GiftCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
