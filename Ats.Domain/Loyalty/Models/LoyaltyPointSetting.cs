using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ats.Domain;

namespace Ats.Domain.Loyalty.Models
{
    [Table("loyaltypointsetting")]
    public class LoyaltyPointSetting : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double PointEarn { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }
        public double RedeemedRate { get; set; }
        public int PointExpiryRule { get; set; }
        public DateTime PointExpiryOn { get; set; }
        public double PointExpiryMonth { get; set; }
        public double LimitEarnPointPerDay { get; set; }
        public double LimitRedeemedPointPerDay { get; set; }
        public bool Active { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Remark { get; set; }
    }
}
