using Ats.Data.EntityFramework;
using Ats.Domain.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Lead
{
    public class LeadRepository : Repository<Domain.Lead.Models.Lead, Guid>, ILeadRepository
    {
        public LeadRepository(SCDataContext context) : base(context)
        {
        }

        public int GetMaxLeadId()
        {
            try
            {
                return this.GetAll().Max(x => x.LeadId);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
