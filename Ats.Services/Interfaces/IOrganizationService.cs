using Ats.Domain.Organization.Models;
using Ats.Models.Organization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IOrganizationService
    {
        List<OrganizationViewModel> Search(string searchText, ORGANIZATION_TYPE? type, string orderField, bool IsAscOrder, int page, int size, out int total);
        OrganizationViewModel GetOrganization(Guid id);
        void CreateOrganization(OrganizationViewModel model);
        void UpdateOrganization(OrganizationViewModel model);
        void DeleteOrganization(Guid id);
    }
}
