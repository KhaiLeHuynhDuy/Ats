using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.MemberWallet
{
   public class MemberWalletHistoryViewModel: BaseSearchViewModel
    {
        public Guid Id { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberCode { get; set; }
        public string BillNo { get; set; }
        public double Amount { get; set; }
        public double Point { get; set; }      
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
        public string ActiveEnds { get; set; }
        public bool Active { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string StoreName { get; set; }
        public string ChannelName { get; set; }
        public int? PointTypeId { get; set; }
        public string refId { get; set; }
        public string TransactionDateString { get; set; }

    }
}
