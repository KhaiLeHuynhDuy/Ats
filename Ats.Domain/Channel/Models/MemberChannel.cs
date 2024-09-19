using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ats.Domain;

namespace Ats.Domain.Channel.Models
{
    [Table("memberchannels")]
    public class MemberChannel : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public string Desc { get; set; }
        public Boolean Active { get; set; } = true;
        [Required]
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveEnd { get; set; }
        
    }
}
