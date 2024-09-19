using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
    public class LoyaltyPointTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool StandardType { get; set; }
        public bool Active { get; set; }
        public string Decs { get; set; }
        public LOYAlTY_POINT_TYPE CampaignType { get; set; }
    }
}
