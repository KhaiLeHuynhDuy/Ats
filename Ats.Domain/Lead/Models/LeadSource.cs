using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    public enum LEAD_SOURCE
    {
        [Display(Name = "--- Chọn ---")]
        NULL = 0,
        [Display(Name = "Nhập dữ liệu")]
        IMPORT = 1,
        [Display(Name = "Đối tác")]
        PARTNER = 2,
        [Display(Name = "Nội bộ")]
        INTERNAL = 3,
    }
}
