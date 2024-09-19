using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class LoanProductAttributeRepository : Repository<LoanProductAttribute, int>, ILoanProductAttributeRepository
    {
        public LoanProductAttributeRepository(SCDataContext context) : base(context)
        {
        }
        public bool CheckExistAddProductAttribute(int productId, int productPropertyId)
        {
            return GetAll().Any(x => x.ProductId.Equals(productId) && x.ProductPropertyId.Equals(productPropertyId));
        }
    }
}
