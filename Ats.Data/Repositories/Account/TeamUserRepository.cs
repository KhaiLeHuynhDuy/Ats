using Ats.Data.EntityFramework;
using Ats.Domain.Account;
using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Account
{
    public class TeamUserRepository : Repository<TeamUser, Guid>, ITeamUserRepository
    {
        public TeamUserRepository(SCDataContext context) : base(context)
        {

        }
        public bool CheckExistAddTeamUser(Guid teamId, Guid userId)
        {
            return GetAll().Any(x => x.TeamId.Equals(teamId) && x.UserId.Equals(userId));
        }
    }
}
