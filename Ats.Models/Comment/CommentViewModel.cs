using Ats.Domain.Lead.Models;
using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Comment
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BorrowerId { get; set; }
        public COMMENT_TYPE CommentType { get; set; }
        public DateTime? CommentDate { get; set; }
        public UserrProfileViewModel User { get; set; }
    }
}
