using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Data.Accounts.Repositories
{
    public class RoleRepository : Repository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(SCDataContext context) : base(context)
        {
        }
    }
}
