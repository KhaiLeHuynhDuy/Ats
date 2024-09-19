using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
  public class MemberLoyaltyDetailViewModel
    {
        public Guid Id { get; set; }
        public int? TierId { get; set; }
        public string TierName { get; set; }
        public Guid MemberId { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
        public bool isValid { get; set; }
        public double UpgradePoint { get; set; }
        public double DowngradePoint { get; set; }
        public string ActiveFromString { get; set; }
        public string ActiveEndString { get; set; }

    }
}
