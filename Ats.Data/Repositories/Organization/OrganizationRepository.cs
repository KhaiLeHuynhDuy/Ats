using Ats.Data.EntityFramework;
using Ats.Domain.Organization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Organization
{
    public class OrganizationRepository : Repository<Domain.Organization.Models.Organization, Guid>, IOrganizationRepository
    {
        public OrganizationRepository(SCDataContext context) : base(context)
        {
        }

    }
}
