using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Member
{

    public class PreferredBrandRepository : Repository<PreferredBrand, Guid>, IPreferredBrandRepository
    {
        public PreferredBrandRepository(SCDataContext context) : base(context)
        {

        }
    }
}
