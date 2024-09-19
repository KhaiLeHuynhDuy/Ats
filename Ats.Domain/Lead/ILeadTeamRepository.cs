using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Lead
{
    public interface ILeadTeamRepository : IRepository<LeadTeam, Guid>
    {
    }
}
