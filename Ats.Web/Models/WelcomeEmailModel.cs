using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Models.Email
{
    public class WelcomeEmailModel
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string ClientName { get; set; }
        public string ApproverName { get; set; }
        public string LeaveType { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string Reason { get; set; }
        public string EmployeeName { get; set; }
        public string State { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string DateSend { get; set; }
        public string Comment { get; set; }
        public string Available { get; set; }
        public string Hours { get; set; }
        public string UrlLink { get; set; }
    }
}
