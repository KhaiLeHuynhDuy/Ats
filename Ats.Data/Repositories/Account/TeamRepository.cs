using Ats.Data.EntityFramework;
using Ats.Domain.Account;
using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Account
{
    public class TeamRepository : Repository<Team, Guid>, ITeamRepository
    {
        public TeamRepository(SCDataContext context) : base(context)
        {
        }
    }
}
