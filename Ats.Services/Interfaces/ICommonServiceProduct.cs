using Ats.Domain.Identity.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Ats.Services.Interfaces
{
    public interface ICommonServiceProduct
    {
        List<ProductCategory> GetProductCategories();
        List<ProductProperty> GetProductProperties();
    }
}
