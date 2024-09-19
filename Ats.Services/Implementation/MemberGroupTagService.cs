using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Domain.Member.Models;
using Ats.Models.Member;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ats.Services.Implementation
{
    public class MemberGroupTagService : BaseService, IMemberGroupTagService
    {
        private IConfiguration _config;
        private int pageSize;
        public MemberGroupTagService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<MemberGroupTagViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Member tag service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.MemberGroupTagRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                           && ((!String.IsNullOrEmpty(x.MemberGroup.MemberGroupName) && x.MemberGroup.MemberGroupName.ToLower().Contains(searchText.ToLower()))
                            || (!String.IsNullOrEmpty(x.MemberTag.TagName) && x.MemberTag.TagName.ToLower().Contains(searchText.ToLower()))
                            || (!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower()))))));

            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "MemberGroupName":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberGroup) : query.OrderByDescending(x => x.MemberGroup);
                        break;
                    case "TagName":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberTag) : query.OrderByDescending(x => x.MemberTag);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberGroupTagViewModel> data = new List<MemberGroupTagViewModel>();
            foreach (var item in datas)
            {
                MemberGroupTagViewModel itemMemberGroupTag = new MemberGroupTagViewModel();
                itemMemberGroupTag.Id = item.Id;
                itemMemberGroupTag.MemberGroupName = item.MemberGroup != null ? item.MemberGroup.MemberGroupName : "";
                itemMemberGroupTag.MemberTagName = item.MemberTag != null ? item.MemberTag.TagName : "";
                itemMemberGroupTag.Remark = item.Remark;
                data.Add(itemMemberGroupTag);
            }
            return data;
        }

        public List<MemberGroupTagViewModel> Gets()
        {
            _logger.LogDebug($"Get Member Group Tags");
            var memberGroupTag = UnitOfWork.MemberGroupTagRepo.GetAll().Select(x => new MemberGroupTagViewModel()
            {
                Id = x.Id,
                MemberGroupName = x.MemberGroup.MemberGroupName,
                MemberTagName = x.MemberTag.TagName,
                Remark = x.Remark
            }).OrderBy(x => x.MemberGroupName).ToList();

            return memberGroupTag;
        }

        public MemberGroupTagViewModel Get(Guid id)
        {
            _logger.LogDebug($"MemberGroupTag Detail service (Id: {id})");
            var entity = UnitOfWork.MemberGroupTagRepo.GetById(id);
            if (entity != null)
            {
                var model = new MemberGroupTagViewModel()
                {
                    Id = entity.Id,
                    MemberGroupId = (Guid)entity.MemberGroupId,
                    MemberTagId = (Guid)entity.MemberTagId,
                    //  Remark = entity.Remark,
                };
                return model;
            }
            return null;
        }

        public Guid Create(MemberGroupTagSearchViewModel model)
        {
            //_logger.LogDebug($"Creating (Name: {model.MemberGroupName})");
            var entity = new MemberGroupTag
            {
                Id = Guid.NewGuid(),
                MemberGroupId = model.MemberGroupTagViewModel.MemberGroupId,
                MemberTagId = model.MemberGroupTagViewModel.MemberTagId,
                //Remark = model.Remark,
            };

            UnitOfWork.MemberGroupTagRepo.Insert(entity);
            UnitOfWork.SaveChanges();
            _logger.LogDebug($"Created (Name: {entity.MemberGroup}, Id: {entity.Id})");
            return entity.Id;
        }

        public void Update(MemberGroupTagViewModel model)
        {
            _logger.LogDebug($"Edit MemberGroupTag (Id: {model.Id})");
            var entity = UnitOfWork.MemberGroupTagRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.MemberGroupId = model.MemberGroupId;
                entity.MemberTagId = model.MemberTagId;
                entity.Remark = model.Remark;
                UnitOfWork.MemberGroupTagRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete  MemberGroupTag(Id: {id})");
            var entity = UnitOfWork.MemberGroupTagRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberGroupTagRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<MemberGroup> GetMemberGroup()
        {
            _logger.LogDebug($"Get MemberGroup Enter.");
            var memberGroup = UnitOfWork.MemberGroupRepo.GetAll().Where(x => x.Active).Select(x => new MemberGroup()
            {
                Id = x.Id,
                MemberGroupName = x.MemberGroupName
            }).OrderBy(x => x.MemberGroupName).ToList();
            return memberGroup;
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
