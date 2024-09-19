using Ats.Data.EntityFramework;
using Ats.Domain.Address;
using System;

namespace Ats.Data.Repositories.Address
{
    public class DistrictRepository : Repository<District, Int32>, IDistrictRepository
    {
        public DistrictRepository(SCDataContext context) : base(context)
        {
        }
    }
}
