using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.MemberWallet;
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
using Ats.Models.Loyalty;

namespace Ats.Services.Implementation
{
    public class LoyaltyPointTypeService : BaseService, ILoyaltyPointTypeService
    {
        private IConfiguration _config;
        private int pageSize;

        public LoyaltyPointTypeService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<LoyaltyPointTypeViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
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
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                    case "standardType":
                        query = IsAscOrder ? query.OrderBy(x => x.StandardType) : query.OrderByDescending(x => x.StandardType);
                        break;
                }
            }

            var data = query.Select(x => new LoyaltyPointTypeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Decs = x.Decs,
                StandardType = x.StandardType,
                Active = x.Active
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<LoyaltyPointTypeViewModel> GetLoyaltyPointTypes()
        {
            _logger.LogDebug($"Get all LoyaltyPointType ");
            var loyaltyPointTypes = UnitOfWork.LoyaltyPointTypeRepo.GetAll().Where(x => x.Active).Select(x => new LoyaltyPointTypeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                StandardType = x.StandardType,
            }).OrderBy(x => x.Name).ToList();

            return loyaltyPointTypes;
        }

        public LoyaltyPointTypeViewModel GetLoyaltyPointType(int id)
        {
            _logger.LogDebug($" LoyaltyPointType Detail service (Id: {id})");
            var entity = UnitOfWork.LoyaltyPointTypeRepo.GetById(id);
            if (entity != null)
            {
                var model = new LoyaltyPointTypeViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    StandardType = entity.StandardType,
                    Decs = entity.Decs,
                    Active = entity.Active
                };

                return model;
            }
            return null;
        }
        public void CreateLoyaltyPointType(LoyaltyPointTypeViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyPointType (Name: {model.Name})");
            var entity = new LoyaltyPointType
            {
                Id = model.Id,
                Name = model.Name,
                StandardType = model.StandardType,
                Decs = model.Decs,
                Active = model.Active
            };

            UnitOfWork.LoyaltyPointTypeRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoyaltyPointType(LoyaltyPointTypeViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyPointType (Id: {model.Id})");
            var entity = UnitOfWork.LoyaltyPointTypeRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Decs = model.Decs;
                entity.StandardType = model.StandardType;
                entity.Active = model.Active;
                UnitOfWork.LoyaltyPointTypeRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoyaltyPointType(int id)
        {
            _logger.LogDebug($"Delete LoyaltyPointType (Id: {id})");
            var entity = UnitOfWork.LoyaltyPointTypeRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoyaltyPointTypeRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
