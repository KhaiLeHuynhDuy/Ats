using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Domain;
using Ats.Domain.Lead.Models;

namespace Ats.Models.Member
{
    public class MemberProductQrCodeViewModel
    { 
        public Guid Id { get; set; } 
        public Guid? MemberId { get; set; }   
        public int? ProductId { get; set; }   
        public string ProductCode { get; set; } 
        public DateTime? Date { get; set; } 
    }
}
