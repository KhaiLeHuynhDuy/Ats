using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
    public class PointRuleChannelRepository : Repository<Ats.Domain.Loyalty.Models.PointRuleChannel, Guid>, IPointRuleChannelRepository
    {
        public PointRuleChannelRepository(SCDataContext context) : base(context)
        {
        }

        public bool CheckExistAddChannels(Guid? ruleId, int? channelId)
        {
            return GetAll().Any(x => x.LoyaltyPointRuleId.Equals(ruleId) && x.ChannelId.Equals(channelId));
        }
    }
}
