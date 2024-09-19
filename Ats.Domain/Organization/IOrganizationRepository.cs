using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Organization
{
    public interface IOrganizationRepository : IRepository<Models.Organization, Guid>
    {
    }
}
