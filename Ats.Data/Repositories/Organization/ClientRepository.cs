using Ats.Data.EntityFramework;
using Ats.Domain.Organization;
using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Organization
{
    public class ClientRepository : Repository<Client, Guid>, IClientRepository
    {
        public ClientRepository(SCDataContext context) : base(context)
        {
        }
    }
}
