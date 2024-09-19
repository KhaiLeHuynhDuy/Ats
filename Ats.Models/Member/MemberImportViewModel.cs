using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Member.Models;
using Microsoft.AspNetCore.Http;

namespace Ats.Models.Member
{
    public class MemberImportViewModel
    {
        public IFormFile ImportFile { get; set; }
        public List<MemberImportItemViewModel> MemberColumnMap { get; set; }
    }
    public class MemberImportItemViewModel
    {
        public int Column { get; set; }
        public MEMBER_IMPORT ColumnDataType { get; set; }
    }

    public class PostDataAndFileMember
    {
        public IFormFile ImageFile { get; set; }
        public string DataInfo { get; set; }
    }
    public class MemberColumnMappingViewModel
    {
        public List<MemberItemViewModel> MemberColumnMap { get; set; }

        public List<MemberExcelViewModel> MemberExport { get; set; }
    }
    public class MemberItemViewModel
    {
        public int? Col { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }

        public MEMBER_IMPORT FieldCode { get { if (!String.IsNullOrEmpty(Field))
                {
                    return (MEMBER_IMPORT)Enum.Parse(typeof(MEMBER_IMPORT), Field);
                }
                return MEMBER_IMPORT.NOT_MAP;
            } }
    }
  
       
}
