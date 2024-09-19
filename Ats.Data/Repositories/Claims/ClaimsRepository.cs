using Ats.Data.EntityFramework;
using Ats.Domain.Claims;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Claims
{
    public class ClaimsRepository :Repository<Ats.Domain.Claims.Models.Claims, int>, IClaimsReprository
    {
        public ClaimsRepository(SCDataContext context) : base(context)
    {
    }
}
}

