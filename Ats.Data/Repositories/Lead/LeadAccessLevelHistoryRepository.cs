using Ats.Data.EntityFramework;
using Ats.Domain.Lead;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Lead
{
    public class LeadAccessLevelHistoryRepository : Repository<LeadAccessLevelHistory, Guid>, ILeadAccessLevelHistoryRepository
    {
        public LeadAccessLevelHistoryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
