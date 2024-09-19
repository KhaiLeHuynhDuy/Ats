using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Lead
{
    public interface ILeadRepository : IRepository<Models.Lead, Guid>
    {
        int GetMaxLeadId();
    }
}
