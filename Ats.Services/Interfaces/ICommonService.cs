using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface ICommonService
    {
        //List<IncomeStream> GetIncomeStreams();
        //List<IncomeAmount> GetIncomeAmounts();
        List<OriginalFile> GetOriginalFiles();
        List<OriginalFileAddition> GetOriginalFileAdditions();
        List<Group> GetRoleGroups();
        List<Organization> GetOrganizations();
        List<LoanProductCategory> GetProductCategories();
        List<AssetType> GetAssetTypes();
        List<LoanProductProperty> GetProductProperties();
        List<AssetProperty> GetAssetProperties();
        List<Warehouse> GetWarehouses();
        List<Loan> GetLoans();
    }
}
