using Ats.Domain.Gift.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Gift
{
    public class GiftCategoryViewModel
    {
        public int Id { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public Boolean Active { get; set; }
        public string Desc { get; set; }
    }
}
