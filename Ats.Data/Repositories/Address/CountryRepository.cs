using Ats.Data.EntityFramework;
using Ats.Domain.Address;
using System;

namespace Ats.Data.Repositories.Address
{
    public class CountryRepository : Repository<Country, Int32>, ICountryRepository
    {
        public CountryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
