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
using Ats.Models.Member;
using Ats.Domain.Member.Models;

namespace Ats.Services.Implementation
{
    public class MemberTagCategoryService : BaseService, IMemberTagCategoryService
    {
        private IConfiguration _config;
        private int pageSize;

        public MemberTagCategoryService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<MemberTagCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            //_logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.MemberTagCategoryRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.TagCategoryName != null && x.TagCategoryName.ToLower().Contains(searchText.ToLower()));
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
                        query = IsAscOrder ? query.OrderBy(x => x.TagCategoryName) : query.OrderByDescending(x => x.TagCategoryName);
                        break;
                    case "remark":
                        query = IsAscOrder ? query.OrderBy(x => x.Remark) : query.OrderByDescending(x => x.Remark);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberTagCategoryViewModel> data = new List<MemberTagCategoryViewModel>();
            foreach (var item in datas)
            {
                MemberTagCategoryViewModel membertagcat = new MemberTagCategoryViewModel();
                membertagcat.Id = item.Id;
                membertagcat.TagCategoryName = item.TagCategoryName;
                membertagcat.Remark = item.Remark;
                membertagcat.Active = item.Active;
                data.Add(membertagcat);
            }

            return data;
        }

        public List<MemberTagCategoryViewModel> Get()
        {
            _logger.LogDebug($"Get all Gift");
            var membertagcat = UnitOfWork.MemberTagCategoryRepo.GetAll().Where(x => x.Active).Select(x => new MemberTagCategoryViewModel()
            {
                Id = x.Id,
                TagCategoryName = x.TagCategoryName
            }).OrderBy(x => x.TagCategoryName).ToList();

            return membertagcat;
        }

        public MemberTagCategoryViewModel Get(int id)
        {
            _logger.LogDebug($"Detail Gift (Id: {id})");
            var entity = UnitOfWork.MemberTagCategoryRepo.GetById(id);
            if (entity != null)
            {
                var model = new MemberTagCategoryViewModel()
                {
                    Id = entity.Id,
                    TagCategoryName = entity.TagCategoryName,
                    Remark = entity.Remark,
                    Active = entity.Active
                };
                return model;
            }
            return null;
        }
        public void Create(MemberTagCategoryViewModel model)
        {
            //_logger.LogDebug($"Create (Name: {model.GiftName})");
            var entity = new MemberTagCategory
            {
                Id = model.Id,
                TagCategoryName = model.TagCategoryName,
                Remark = model.Remark,
                Active = model.Active
            };

            UnitOfWork.MemberTagCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void Update(MemberTagCategoryViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.MemberTagCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Remark = model.Remark;
                entity.TagCategoryName = model.TagCategoryName;
                entity.Active = model.Active;
                UnitOfWork.MemberTagCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.MemberTagCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberTagCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

       
    }
}
