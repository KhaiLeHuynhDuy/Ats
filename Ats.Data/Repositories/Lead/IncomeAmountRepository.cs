using Ats.Data.EntityFramework;
using Ats.Domain.Lead;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Lead
{
    public class IncomeAmountRepository : Repository<IncomeAmount, int>, IIncomeAmountRepository
    {
        public IncomeAmountRepository(SCDataContext context) : base(context)
        {
        }
    }
}
