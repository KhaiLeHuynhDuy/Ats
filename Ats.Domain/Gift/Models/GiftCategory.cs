using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Gift.Models
{
    [Table("giftcategories")]
    public class GiftCategory : AuditBase, IEntity<int>
    {
        public int Id { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public Boolean Active { get; set; } = true;

        public string Desc { get; set; }
    }
}
