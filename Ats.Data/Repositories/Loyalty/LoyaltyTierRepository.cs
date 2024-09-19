using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;
using System.Linq;

namespace Ats.Data.Repositories.Loyalty
{
    
    public class LoyaltyTierRepository : Repository<Ats.Domain.Loyalty.Models.LoyaltyTier, int>, ILoyaltyTierRepository
    {
        public LoyaltyTierRepository(SCDataContext context) : base(context)
        {
        }

        public int GetLowestLoyaltyTierLevel()
        {
            return this.GetAll().Where(x => x.Active).OrderBy(x => x.TierLevel).Select(x => x.Id).FirstOrDefault();
        }
    }
}
