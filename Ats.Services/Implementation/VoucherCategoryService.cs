using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Domain.Voucher.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Voucher;
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
    public class VoucherCategoryService : BaseService, IVoucherCategoryService
    {
        private IConfiguration _config;
        private int pageSize;

        public VoucherCategoryService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<VoucherCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.VoucherCategoryRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.VoucherName != null && x.VoucherName.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.VoucherCode) : query.OrderByDescending(x => x.VoucherCode);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.VoucherName) : query.OrderByDescending(x => x.VoucherName);
                        break;
                }
            }

            var data = query.Select(x => new VoucherCategoryViewModel()
            {
                Id = x.Id,
                VoucherCode = x.VoucherCode,
                VoucherName = x.VoucherName,
                Desc = x.Desc,
                Active = x.Active
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<VoucherCategoryViewModel> GetVoucherCategorys()
        {
            _logger.LogDebug($"Get all voucher category");
            var vouchers = UnitOfWork.VoucherCategoryRepo.GetAll().Where(x => x.Active).Select(x => new VoucherCategoryViewModel()
            {
                Id = x.Id,
                VoucherCode = x.VoucherCode,
                VoucherName = x.VoucherName
            }).OrderBy(x => x.VoucherName).ToList();

            return vouchers;
        }

        public VoucherCategoryViewModel GetVoucherCategory(int id)
        {
            _logger.LogDebug($"Detail service (Id: {id})");
            var entity = UnitOfWork.VoucherCategoryRepo.GetById(id);
            if (entity != null)
            {
                var model = new VoucherCategoryViewModel()
                {
                    Id = entity.Id,
                    VoucherCode = entity.VoucherCode,
                    VoucherName = entity.VoucherName,
                    Desc = entity.Desc,
                    Active = entity.Active
                };

                return model;
            }
            return null;
        }
        public void CreateVoucherCategory(VoucherCategoryViewModel model)
        {
            _logger.LogDebug($"Create (Name: {model.VoucherName})");
            var entity = new VoucherCategory
            {
                Id = model.Id,
                VoucherCode = model.VoucherCode,
                VoucherName = model.VoucherName,
                Desc = model.Desc,
                Active = model.Active
            };

            UnitOfWork.VoucherCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateVoucherCategory(VoucherCategoryViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.VoucherCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.VoucherCode = model.VoucherCode;
                entity.VoucherName = model.VoucherName;
                entity.Desc = model.Desc;
                entity.Active = model.Active;
                UnitOfWork.VoucherCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteVoucherCategory(int id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.VoucherCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.VoucherCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
