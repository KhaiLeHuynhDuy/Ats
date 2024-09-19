using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Brand
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string Desc { get; set; }
        public Boolean Active { get; set; }
    }
}
