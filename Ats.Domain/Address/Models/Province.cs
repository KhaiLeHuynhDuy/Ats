using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Address
{
    [Table("provinces")]
    public class Province : IEntity<int>
    {
      
        [Key]
        public int Id { get; set; }

        [StringLength(8, ErrorMessage = "ProvinceCode cannot be longer than 8 characters.")]
        public string ProvinceCode { get; set; }

        [StringLength(256, ErrorMessage = "Name cannot be longer than 256 characters.")]
        public string Name { get; set; }

        public string CountryCode { get; set; }

    }
}
