using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Organization
{
    public class OrganizationSearchViewModel : BaseSearchViewModel
    {
        public List<OrganizationViewModel> Organizations { get; set; }
        public OrganizationViewModel Organization { get; set; }
        public ORGANIZATION_TYPE? Type { get; set; }
    }
}
