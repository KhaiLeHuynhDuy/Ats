using Ats.Data.EntityFramework;
using Ats.Domain.Lead;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Lead
{
    public class LeadStatusHistoryRepository : Repository<LeadStatusHistory, Guid>, ILeadStatusHistoryRepository
    {
        public LeadStatusHistoryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
