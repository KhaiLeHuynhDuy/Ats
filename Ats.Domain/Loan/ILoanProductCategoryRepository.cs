﻿using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loan
{
    public interface ILoanProductCategoryRepository : IRepository<LoanProductCategory, int>
    {
    }
}