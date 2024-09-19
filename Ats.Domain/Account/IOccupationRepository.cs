using Ats.Domain.Account.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Account
{
    public interface IOccupationRepository : IRepository<Occupation, int>
    {
        int GetMaxOccupationId();
    }
}
