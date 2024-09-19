using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Organization.Models
{
    [Table("lenders")]
    public class Lender : AuditBase, IEntity<int>
    {
        public Lender()
        {
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
