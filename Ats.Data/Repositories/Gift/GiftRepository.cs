using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Gift;

namespace Ats.Data.Repositories.Gift
{
    public class GiftRepository : Repository<Ats.Domain.Gift.Models.Gift, Guid>, IGiftRepository
    {
        public GiftRepository(SCDataContext context) : base(context)
        {
        }
    }
}
