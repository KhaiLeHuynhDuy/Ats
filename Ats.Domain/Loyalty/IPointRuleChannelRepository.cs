using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loyalty
{
   public interface IPointRuleChannelRepository : IRepository<Models.PointRuleChannel,Guid>
    {
        bool CheckExistAddChannels(Guid? ruleId, int? channelId);
    }
}
