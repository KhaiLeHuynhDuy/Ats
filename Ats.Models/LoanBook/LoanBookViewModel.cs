using Ats.Domain.Lead.Models;
using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.LoanBook
{
    public class LoanBookViewModel
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
        public BOOK_TYPE BookType { get; set; }
        public DateTime? BookDate { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? BorrowerId { get; set; }
        public Guid? UserId { get; set; }
        public int? LoanProductId { get; set; }
        public UserrProfileViewModel User { get; set; }
    }
}
