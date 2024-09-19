using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Gift;

namespace Ats.Data.Repositories.Gift
{
    public class MemberGiftRepository : Repository<Ats.Domain.Gift.Models.MemberGift, Guid>, IMemberGiftRepository
    {
        public MemberGiftRepository(SCDataContext context) : base(context)
        {
        }
    }
}
