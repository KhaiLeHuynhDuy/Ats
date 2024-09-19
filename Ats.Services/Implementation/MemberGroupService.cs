using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Member;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ats.Services.Implementation
{
    public class MemberGroupService : BaseService, IMemberGroupService
    {
        private IConfiguration _config;
        private int pageSize;

        public MemberGroupService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<MemberGroupViewModel> Search(string searchText, int? MemberGroupId, int? MemberTagId, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Member group service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.MemberGroupRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                           && ((!String.IsNullOrEmpty(x.MemberGroupName) && x.MemberGroupName.ToLower().Contains(searchText.ToLower()))                         
                            || (!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower()))))));

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
                    case "conditional":
                        query = IsAscOrder ? query.OrderBy(x => x.Conditional) : query.OrderByDescending(x => x.Conditional);
                        break;
                    case "membergroupname":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberGroupName) : query.OrderByDescending(x => x.MemberGroupName);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberGroupViewModel> data = new List<MemberGroupViewModel>();
            foreach (var item in datas)
            {
                MemberGroupViewModel itemMembergroup= new MemberGroupViewModel();
                itemMembergroup.Id = item.Id;
                itemMembergroup.MemberGroupName = item.MemberGroupName;
                itemMembergroup.Conditional = item.Conditional;
                itemMembergroup.Active = item.Active;
                itemMembergroup.Remark = item.Remark;            
                data.Add(itemMembergroup);
            }
            return data;
        }

        public List<MemberGroupViewModel> GetMemberGroups()
        {
            _logger.LogDebug($"Get MemberGroups");
            var memberGroup = UnitOfWork.MemberGroupRepo.GetAll().Where(x => x.Active).Select(x => new MemberGroupViewModel()
            {
                Id = x.Id,
                MemberGroupName = x.MemberGroupName,
                Conditional = x.Conditional,
                Active = x.Active,
                Remark = x.Remark,
            }).OrderBy(x => x.MemberGroupName).ToList();

            return memberGroup;
        }

        public MemberGroupViewModel GetMemberGroup(Guid id)
        {
            _logger.LogDebug($"MemberGroup Detail service (Id: {id})");
            MemberGroup entity = UnitOfWork.MemberGroupRepo.GetById(id);
            if (entity != null)
            {
                var model = new MemberGroupViewModel()
                {
                    Id = entity.Id,
                    MemberGroupName = entity.MemberGroupName,
                    Conditional = entity.Conditional,
                    Remark = entity.Remark,
                    Active = entity.Active
                };

                model.Tags = entity.MemberGroupTags.Select(x => new DisplayItem() { 
                    Value = x.MemberTagId.ToString(),
                    Selected = true
                }).ToList();

                foreach (MemberGroupTag tag in entity.MemberGroupTags)
                {

                }
                return model;
            }
            return null;
        }

        public Guid Create(MemberGroupViewModel model)
            {
            _logger.LogDebug($"Creating (Name: {model.MemberGroupName})");
            var entity = new MemberGroup
            {
                Id = Guid.NewGuid(),
                MemberGroupName = model.MemberGroupName,
                Conditional = model.Conditional,
                Remark = model.Remark,
                Active = model.Active
            };

            // Tags
            foreach(DisplayItem item in model.Tags)
            {
                if (item.Selected)
                {
                    Guid tagId = Guid.NewGuid();
                    if (Guid.TryParse(item.Value, out tagId))
                    {
                        var tag = new MemberGroupTag()
                        {
                            Id = Guid.NewGuid(),
                            MemberGroupId = entity.Id,
                            MemberTagId = tagId
                        };
                        UnitOfWork.MemberGroupTagRepo.Insert(tag);
                    }
                }
            }

            UnitOfWork.MemberGroupRepo.Insert(entity);
            UnitOfWork.SaveChanges();

            _logger.LogDebug($"Created (Name: {entity.MemberGroupName}, Id: {entity.Id})");

            return entity.Id;
        }

        public void Update(MemberGroupViewModel model)
        {
            _logger.LogDebug($"Edit MemberTag (Id: {model.Id})");
            var entity = UnitOfWork.MemberGroupRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.MemberGroupName = model.MemberGroupName;
                entity.Conditional = model.Conditional;         
                entity.Remark = model.Remark;
                entity.Active = model.Active;
            
                UnitOfWork.MemberGroupRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete  MemberGroup(Id: {id})");
            var entity = UnitOfWork.MemberGroupRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberGroupRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<MemberTag> GetMemberTag()
        {
            _logger.LogDebug($"Get MemberTag Enter.");
            var memberTag = UnitOfWork.MemberTagRepo.GetAll().Where(x => x.Active).Select(x => new MemberTag()
            {
                Id = x.Id,
                TagName = x.TagName
            }).OrderBy(x => x.TagName).ToList();
            return memberTag;
        }
    }
}
