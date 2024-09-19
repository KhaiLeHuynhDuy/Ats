using Ats.Domain.Lead.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Document
{
    public class DocumentViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public string Remark { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ValidTo { get; set; }
        public Guid? LeadId { get; set; }
        public Guid? BorrowerId { get; set; }
        public int LeadNum { get; set; }
        public int BorrowerNum { get; set; }
        public DOCUMENT_TYPE DocumentType { get; set; }
        public IFormFile File { get; set; }
    }
}
