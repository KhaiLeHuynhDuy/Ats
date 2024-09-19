using Ats.Commons.Utilities;
using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Gift.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Member;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Ats.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Services
{
    public class MemberLifecycleService : BaseService, IMemberLifecycleService
    {
        private IConfiguration _config;
        private int pageSize;

        public MemberLifecycleService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<MemberLifecycleViewModel> Search(string keyword)
        {
            _logger.LogDebug($"Get all Member life cycle service Search={keyword}");
            if (!String.IsNullOrEmpty(keyword)) keyword = keyword.Trim();
            var query = UnitOfWork.MemberLifeCycleSettingRepo.GetAll().Where(x => (String.IsNullOrEmpty(keyword) || (!String.IsNullOrEmpty(keyword)
                           && ((!String.IsNullOrEmpty(x.MemberLifeCycleName) && x.MemberLifeCycleName.ToLower().Contains(keyword.ToLower()))     
                            ))));
            
            var datas = query.Select(x=>new MemberLifecycleViewModel() { 
                                    Id = x.Id,
                                    MemberLifeCycleName =  x.MemberLifeCycleName,
                                    LastRegisterMonthDuration = x.LastRegisterMonthDuration,
                                    TotalTransaction = x.TotalTransaction,
                                    DisplayOrder = x.DisplayOrder,
                                    Lifecycle = x.Lifecycle,
                                    Remark = x.Remark,
                                    Active = x.Active
            }).OrderBy(x=>x.DisplayOrder).ToList();

            return datas;
        }

        public List<MemberLifecycleViewModel> GetMemberLifecycles()
        {       
            _logger.LogDebug($"Get MemberLifecycles");
            var memberLifecycles = UnitOfWork.MemberLifeCycleSettingRepo.GetAll().Where(x => x.Active).Select(x => new MemberLifecycleViewModel()
            {
                Id = x.Id,
                MemberLifeCycleName = x.MemberLifeCycleName,
                LastRegisterMonthDuration = x.LastRegisterMonthDuration,
                TotalTransaction = x.TotalTransaction,
                DisplayOrder = x.DisplayOrder,
                Lifecycle = x.Lifecycle,
                Remark = x.Remark, 
            }).OrderBy(x => x.MemberLifeCycleName).ToList(); 
            return memberLifecycles;
        }      

        public MemberLifecycleViewModel GetMemberLifecycle(int id)
        {         
            _logger.LogDebug($"MemberLifecycle Detail service (Id: {id})");
            var entity = UnitOfWork.MemberLifeCycleSettingRepo.GetById(id);
            if (entity != null)
            {
                var model = new MemberLifecycleViewModel()
                {
                    Id = entity.Id,
                    MemberLifeCycleName = entity.MemberLifeCycleName,
                    LastRegisterMonthDuration = entity.LastRegisterMonthDuration,
                    TotalTransaction = entity.TotalTransaction,
                    DisplayOrder = entity.DisplayOrder,
                    Lifecycle = entity.Lifecycle,
                    Remark = entity.Remark
                };
                return model;
            }
            return null;
        } 
        public int Create(MemberLifecycleViewModel model)
        {
            _logger.LogDebug($"Creating (Name: {model.MemberLifeCycleName})");
            var entity = new MemberLifeCycle 
            {
                Id = model.Id,
                MemberLifeCycleName = model.MemberLifeCycleName,
                LastRegisterMonthDuration = model.LastRegisterMonthDuration,
                TotalTransaction = model.TotalTransaction,
                DisplayOrder = model.DisplayOrder,
                Lifecycle = model.Lifecycle,
                Remark = model.Remark
            };
            UnitOfWork.MemberLifeCycleSettingRepo.Insert(entity);
            UnitOfWork.SaveChanges();

            _logger.LogDebug($"Created (Name: {entity.MemberLifeCycleName}, Id: {entity.Id})");

            return entity.Id;
        }

        public void Update(MemberLifecycleViewModel model)
        {
            _logger.LogDebug($"Edit MemberLifecycle (Id: {model.Id})");
            var entity = UnitOfWork.MemberLifeCycleSettingRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.MemberLifeCycleName = model.MemberLifeCycleName;
                entity.LastRegisterMonthDuration = model.LastRegisterMonthDuration;
                entity.DisplayOrder = model.TotalTransaction;
                entity.DisplayOrder = model.DisplayOrder; 
                entity.Lifecycle = model.Lifecycle; 
                entity.Remark = model.Remark; 
                UnitOfWork.MemberLifeCycleSettingRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void Update(List<MemberLifecycleViewModel> model)
        {
            _logger.LogDebug($"Update MemberLifecycle (Total: {model.Count} items)");

            foreach(MemberLifecycleViewModel item in model)
            {
                var entity = UnitOfWork.MemberLifeCycleSettingRepo.GetById(item.Id);
                if(entity != null)
                {
                    entity.LastRegisterMonthDuration = item.LastRegisterMonthDuration;
                    entity.TotalTransaction = item.TotalTransaction;

                    //this.UnitOfWork.MemberLifeCycleSettingRepo.Update(entity, x=> new { x.LastRegisterMonthDuration, x.TotalTransaction });
                    this.UnitOfWork.MemberLifeCycleSettingRepo.Update(entity, x=> x.LastRegisterMonthDuration, x => x.TotalTransaction );
                }
            }
            this.UnitOfWork.SaveChanges();
        }
    }
}