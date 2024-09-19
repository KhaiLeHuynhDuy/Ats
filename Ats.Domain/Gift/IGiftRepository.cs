using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Gift
{
    public interface IGiftRepository : IRepository<Models.Gift, Guid>
    {
    }
}
