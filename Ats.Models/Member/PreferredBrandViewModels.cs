using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    class PreferredBrandViewModels
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public int BrandId { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
    }
}
