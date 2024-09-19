using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
    public class LoyaltyPointSettingRepository : Repository<Ats.Domain.Loyalty.Models.LoyaltyPointSetting, int>, ILoyaltyPointSettingRepository
    {
        public LoyaltyPointSettingRepository(SCDataContext context) : base(context)
        {
        }
    }
}
