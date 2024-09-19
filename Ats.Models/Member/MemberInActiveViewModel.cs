using Ats.Domain;
using Ats.Domain.Lead.Models;
using Ats.Models.Brand;
using Ats.Models.Comment;
using Ats.Models.Document;
using Ats.Models.MemberWallet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ats.Models.Member
{
    public class MemberInActiveViewModel
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; } 
        public DateTime CreateDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public bool Active { get; set; }
    }
}
