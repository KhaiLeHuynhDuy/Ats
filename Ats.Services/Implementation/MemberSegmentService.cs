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
    public class MemberSegmentService : BaseService, IMemberSegmentService
    {
        private IConfiguration _config;

        public MemberSegmentService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
        }

        public List<MemberSegmentViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Member Segment: {searchText}, Page: {page}, Size: {size}");

            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();

            var query = UnitOfWork.MemberSegmentRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                           && ((!String.IsNullOrEmpty(x.MemberSegmentName) && x.MemberSegmentName.ToLower().Contains(searchText.ToLower()))))));

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
                    case "membersegmentname":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberSegmentName) : query.OrderByDescending(x => x.MemberSegmentName);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();

            List<MemberSegmentViewModel> data = new List<MemberSegmentViewModel>();
            foreach (var item in datas)
            {
                MemberSegmentViewModel segment = new MemberSegmentViewModel();
                segment.Id = item.Id;
                segment.SegmentName = item.MemberSegmentName;
                segment.Conditional = item.Conditional;
                segment.Active = item.Active;
                segment.Remark = item.Remark;            
                data.Add(segment);
            }
            return data;
        }

        public List<MemberSegmentViewModel> GetMemberSegments()
        {
            _logger.LogDebug($"Get MemberSegments");
            var resutl = UnitOfWork.MemberSegmentRepo.GetAll().Where(x => x.Active).Select(x => new MemberSegmentViewModel()
            {
                Id = x.Id,
                SegmentName = x.MemberSegmentName,
                Conditional = x.Conditional,
                Active = x.Active,
                Remark = x.Remark,
            }).OrderBy(x => x.SegmentName).ToList();

            return resutl;
        }

        public MemberSegmentViewModel GetMemberSegment(Guid id)
        {
            _logger.LogDebug($"MemberSegment (Id: {id})");

            MemberSegment entity = UnitOfWork.MemberSegmentRepo.GetById(id);
            if (entity != null)
            {
                var model = new MemberSegmentViewModel()
                {
                    Id = entity.Id,
                    SegmentName = entity.MemberSegmentName,
                    Conditional = entity.Conditional,
                    Remark = entity.Remark,
                    Active = entity.Active
                };                
                return model;
            }

            return null;
        }

        public Guid Create(MemberSegmentViewModel model)
        {
            _logger.LogDebug($"Creating MemberSegment (Name: {model.SegmentName})");

            var entity = new MemberSegment
            {
                Id = Guid.NewGuid(),
                MemberSegmentName = model.SegmentName,
                Conditional = model.Conditional,
                Remark = model.Remark,
                Active = model.Active
            };

            UnitOfWork.MemberSegmentRepo.Insert(entity);
            UnitOfWork.SaveChanges();

            _logger.LogDebug($"Created (Name: {entity.MemberSegmentName}, Id: {entity.Id})");

            return entity.Id;
        }

        public void Update(MemberSegmentViewModel model)
        {
            _logger.LogDebug($"Edit MemberSegment (Id: {model.Id})");
            
            var entity = UnitOfWork.MemberSegmentRepo.GetById(model.Id);

            if (entity != null)
            {
                entity.MemberSegmentName = model.SegmentName;
                entity.Conditional = model.Conditional;         
                entity.Remark = model.Remark;
                entity.Active = model.Active;

                UnitOfWork.MemberSegmentRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete  MemberSegment(Id: {id})");
            var entity = UnitOfWork.MemberSegmentRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberSegmentRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
