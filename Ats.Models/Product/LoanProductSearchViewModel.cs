using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class LoanProductSearchViewModel : BaseSearchViewModel
    {
        public List<LoanProductViewModel> Products { get; set; }
        public LoanProductViewModel Product { get; set; }
    }
}
