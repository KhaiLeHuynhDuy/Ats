using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Claims.Models
{
    [Table("claims")]
    public class Claims : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
      

    }
}

