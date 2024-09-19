using Ats.Domain.Lead.Models;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Collections.Generic;

namespace Ats.Models.Lead
{
    public class LeadImportViewModel
    {
        public string LableColA { get; set; }
        public string LableColB { get; set; }
        public string LableColC { get; set; }
        public string LableColD { get; set; }
        public string LableColE { get; set; }
        public string LableColF { get; set; }
        public LEAD_IMPORT ColA { get; set; }
        public LEAD_IMPORT ColB { get; set; }
        public LEAD_IMPORT ColC { get; set; }
        public LEAD_IMPORT ColD { get; set; }
        public LEAD_IMPORT ColE { get; set; }
        public LEAD_IMPORT ColF { get; set; }
        public IFormFile FormFile { get; set; }
        public List<LeadImportItemViewModel> Leads { get; set; }
    }

    public class LeadImportItemViewModel
    {
        public string LableColumn { get; set; }
        public LEAD_IMPORT ValueColumn { get; set; }
    }

    public class PostDataAndFile
    {
        public IFormFile ImageFile { get; set; }
        public string DataInfo { get; set; }
    }
    public class LeadsViewModel
    {
        public List<LeadItemViewModel> LeadItems { get; set; }
    }
    public class LeadItemViewModel
    {
        public string Col { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }
    }
}
