using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Models.Loyalty;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ats.Services.Implementation
{
   public class PointRuleStoresService : BaseService, IPointRuleStoresService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public PointRuleStoresService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

             
        public List<PointRuleStoresViewModel> Gets()
        {
            _logger.LogDebug($"Get all Staore");
            var stores = UnitOfWork.PointRuleStoresRepo.GetAll().Where(x => x.Valid).Select(x => new PointRuleStoresViewModel()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                StoreName = x.Store.StoreName,
                LoyaltyPointRuleId = x.LoyaltyPointRuleId,
                LoyaltyPointRuleName = x.LoyaltyPointRule.RuleName,
                //EffectiveDateBegin = string.Format("{0:dd/MM/yyyy}", x.EffectiveFrom),
            }).OrderBy(x => x.StoreId).ToList();
            return stores;
        }




      
    }
}
