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
   public class PointRuleChannelService : BaseService, IPointRuleChannelService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public PointRuleChannelService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

    
        public List<PointRuleChannelViewModel> GetPointRuleChannels()
        {
            _logger.LogDebug($"Get all Staore");
            var channels = UnitOfWork.PointRuleChannelRepo.GetAll().Where(x => x.Valid).Select(x => new PointRuleChannelViewModel()
            {
                Id = x.Id,
                ChannelId = x.ChannelId,
                ChannelName = x.Channel.ChannelName,
                LoyaltyPointRuleId = x.LoyaltyPointRuleId,
                LoyaltyPointRuleName = x.LoyaltyPointRule.RuleName,
                //EffectiveFroms = string.Format("{0:dd/MM/yyyy}", x.EffectiveFrom),

            }).OrderBy(x => x.ChannelId).ToList();
            return channels;
        }

      
    }
}
