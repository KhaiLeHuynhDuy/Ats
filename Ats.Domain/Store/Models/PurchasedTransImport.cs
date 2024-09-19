using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Domain.Store.Models
{

    public enum PURCHASEDTRANS_IMPORT
    {
        [Display(Name = "")]
        NOT_MAP = 0,
        [Display(Name = "Invoice No")]
        INVOICE_NO = 1,
        [Display(Name = "Ref Id")]
        REF_ID = 2,
        [Display(Name = "Amount")]
        AMOUNT = 3,
        [Display(Name = "Purchased Date")]
        PURCHASED_DATE = 4, 
        [Display(Name = "Currency")]
        CURRENCY = 5,
        [Display(Name = "Member Reference")]
        MEMBER_REFERENCE = 6,
        [Display(Name = "Remark")]
        REMARK = 7,
        
    }
}
