using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Address
{
    [Table("countries")]
    public class Country : IEntity<Int32>
    {
        [Key]
        public int Id { get; set; }

        public string Iso { get; set; }

        [StringLength(128, ErrorMessage = "Name cannot be longer than 128 characters.")]
        public string Name { get; set; }

        public string Iso3 { get; set; }

        public string NumCode { get; set; }

        public string PhoneCode { get; set; }
    }
}
