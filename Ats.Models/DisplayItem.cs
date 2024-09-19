
using Ats.Domain.Store.Models;
using System;

namespace Ats.Models
{
    public class DisplayItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public double Price { get; set; }
        public bool Selected { get; set; }

        // use for product order
        //public Guid Id { get; set; }
        public string MemberCode { get; set; }
        public string NameMember { get; set; }
        public string zaloId { get; set; }
        public string NameProduct { get; set; }
        public string Receiver { get; set; }
        public string PhoneNo { get; set; }
        public string ShippingAddress { get; set; }
        public string DateCreate { get; set; }
        public double Quantity { get; set; }
        public int IdStatus { get; set; }
        public Guid IdMember { get; set; }
        public Guid IdProductItem { get; set; }
        public PRODUCT_ORDER_STATUS OrderStatus { get; set; }

    }
}
