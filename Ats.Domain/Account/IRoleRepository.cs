using Ats.Domain;
using System;
using System.Collections.Generic;
using Ats.Domain.Identity.Models;

namespace Ats.Domain.Accounts.Repositories
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
    }
}
