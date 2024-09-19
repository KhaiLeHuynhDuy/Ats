using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Voucher.Models
{
    public enum VOUCHER_TEMPLATE {
        TEXT = 1,
        QR = 2,
        BARCODE = 3,
        QR_AND_BARCODE = 4
    };

    [Table("vouchers")]
    public class Voucher : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public int? VoucherCategoryId { get; set; }
        [ForeignKey("VoucherCategoryId")]
        public virtual VoucherCategory VoucherCategory { get; set; }

        public VOUCHER_TEMPLATE Template { get; set; }

        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Ats.Domain.Channel.Models.MemberChannel Channel { get; set; }

        public bool VoucherType { get; set; } // 0 discount, 1 cash

        public double Amount { get; set; }
        public double Discount { get; set; }

        public double FaceValue { get; set; }
        public double Price { get; set; }

        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }
        public int MinimumPurchase { get; set; }

        public string TermAndConditions { get; set; }

        public int AvailableStock { get; set; } // no-limit (9999999)
        public string TemplateBanner { get; set; }

        public Boolean Active { get; set; } = true;
        public string Desc { get; set; }

    }
}
