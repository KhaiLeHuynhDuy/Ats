using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
   public class ProductQrReportsSearchViewModel : BaseSearchProductPointsScanViewModel
    {
        public List<ProductQrReportsViewModel> productQrReports { get; set; }
        public ProductQrReportsViewModel productQr { get; set; }

        public string[] ArProductId { get; set; }
        public IEnumerable<SelectListItem> ProductIdList { get; set; }
      
        public string Stringdatefrom { get; set; }
        public string Stringdateto { get; set; }
        public string ProductName { get; set; }

    }
}
