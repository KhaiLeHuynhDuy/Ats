using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Address.Models
{
    [Table("addresses")]
    public class Address : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(256, ErrorMessage = "Address1 cannot be longer than 256 characters.")]
        public string Address1 { get; set; }

        [StringLength(256, ErrorMessage = "Address2 cannot be longer than 256 characters.")]
        public string Address2 { get; set; }

        public Int32? DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }

        public Int32? ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        public Int32? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public string CountryCode { get; set; }
    }
}
