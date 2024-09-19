using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Channel;

namespace Ats.Data.Repositories.Channel
{
    public class MemberChannelRepository : Repository<Ats.Domain.Channel.Models.MemberChannel, int>, IMemberChannelRepository
    {
        public MemberChannelRepository(SCDataContext context) : base(context)
        {
        }
    }
}
