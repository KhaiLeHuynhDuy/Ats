using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Account
{
    public interface ITeamUserRepository : IRepository<TeamUser, Guid>
    {
        bool CheckExistAddTeamUser(Guid teamId, Guid userId);
    }
}
