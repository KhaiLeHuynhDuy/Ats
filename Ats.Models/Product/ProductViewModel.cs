using Ats.Domain;
using Ats.Domain.Store.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
       
        public int UnitId { get; set; }
        public string UnitCategoryName { get; set; }
        public virtual Ats.Domain.Store.Models.Unit Unit { get; set; }
        public double RetailPrice { get; set; }
        public double PurchasedPrice { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageProfile { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string uniqueFileName { get; set; }

        public bool AllowEarnPoint { get; set; }
        public bool AllowRedeem { get; set; }
    }
}
