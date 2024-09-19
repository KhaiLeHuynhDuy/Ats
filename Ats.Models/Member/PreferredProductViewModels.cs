using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    class PreferredProductViewModels
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public int ProductID { get; set; }
        public DateTime? ActiveFrom { get; set; } 
        public DateTime? ActiveEnd { get; set; } 
    }
}
