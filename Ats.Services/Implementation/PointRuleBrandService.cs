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
   public class PointRuleBrandService : BaseService, IPointRuleBrandService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public PointRuleBrandService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

        public List<PointRuleBrandViewModel> Gets()
        {
            _logger.LogDebug($"Get all Product");
            var brands = UnitOfWork.PointRuleBrandRepo.GetAll().Where(x => x.Valid).Select(x => new PointRuleBrandViewModel()
            {
                Id = x.Id,
                //BrandId = x.BrandId,
                //BrandName = x.Brand.BrandName,
                LoyaltyPointRuleId = x.LoyaltyPointRuleId,
                LoyaltyPointRuleName = x.LoyaltyPointRule.RuleName,
                //EffectiveFroms = string.Format("{0:dd/MM/yyyy}", x.EffectiveFrom),

            }).OrderBy(x => x.BrandId).ToList();
            return brands;
        }
    }
}
