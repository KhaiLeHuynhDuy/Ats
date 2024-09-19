using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ats.Domain.Identity.Models;

namespace Ats.Domain.Address
{
    [Table("districts")]
    public class District : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256, ErrorMessage = "Name cannot be longer than 256 characters.")]
        public string Name { get; set; }

        public int ProvinceId { get; set; }
        //public virtual Province Province { get; set; }
        public string ProvinceCode { get; set; }
    }
}
