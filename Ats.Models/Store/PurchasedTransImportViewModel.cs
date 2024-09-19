using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Microsoft.AspNetCore.Http;

namespace Ats.Models.Store
{
    public class PurchasedTransImportViewModel
    {
        public IFormFile ImportFile { get; set; }
        public List<PurchasedTransImportItemViewModel> PurchasedTransColumnMap { get; set; }
    }
    public class PurchasedTransImportItemViewModel
    {
        public int Column { get; set; }
        public PURCHASEDTRANS_IMPORT ColumnDataType { get; set; }
    }

    public class PostDataAndFilePurchasedTrans
    {
        public IFormFile ImageFile { get; set; }
        public string DataInfo { get; set; }
    }
    public class PurchasedTransColumnMappingViewModel
    {
        public List<PurchasedTransItemViewModel> PurchasedTransColumnMap { get; set; }
    }
    public class PurchasedTransItemViewModel
    {
        public int? Col { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }

        public PURCHASEDTRANS_IMPORT FieldCode { get { if (!String.IsNullOrEmpty(Field)) 
                { 
                    return (PURCHASEDTRANS_IMPORT)Enum.Parse(typeof(PURCHASEDTRANS_IMPORT), Field); 
                }
                return PURCHASEDTRANS_IMPORT.NOT_MAP;
            } }
    }
}
