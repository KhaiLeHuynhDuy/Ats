using Ats.Data.EntityFramework;
using Ats.Domain.Address;
using System;

namespace Ats.Data.Repositories.Address
{
    public class ProvinceRepository : Repository<Province, Int32>, IProvinceRepository
    {
        public ProvinceRepository(SCDataContext context) : base(context)
        {
        }
    }
}
