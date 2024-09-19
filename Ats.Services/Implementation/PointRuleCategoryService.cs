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
   public class PointRuleCategoryService : BaseService, IPointRuleCategoryService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public PointRuleCategoryService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

        public List<PointRuleCategoryViewModel> Gets()
        {
            _logger.LogDebug($"Get all Category");
            var brands = UnitOfWork.PointRuleCategoryRepo.GetAll().Where(x => x.Valid).Select(x => new PointRuleCategoryViewModel()
            {
                Id = x.Id,
                ProductCateId = x.ProductCateId,
                ProductCategoriesName = x.ProductCategory.Name,
                LoyaltyPointRuleId = x.LoyaltyPointRuleId,
                LoyaltyPointRuleName = x.LoyaltyPointRule.RuleName,
                //EffectiveFroms = string.Format("{0:dd/MM/yyyy}", x.EffectiveFrom),

            }).OrderBy(x => x.ProductCateId).ToList();
            return brands;
        }
    }
}
