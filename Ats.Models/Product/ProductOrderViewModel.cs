using Ats.Domain;
using Ats.Domain.Store.Models;
using Ats.Models.Member;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{

    public class ProductOrderViewModel
    {
       


        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }   
        public Guid MemberId { get; set; }   
        public DateTime? Date { get; set; }
        public string DateOrder { get; set; }
        public string DateCreate { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Point { get; set; }
        public string Remark { get; set; }
        public PRODUCT_ORDER_STATUS OrderStatus { get; set; }
        public string ShippingAddress { get; set; }
        public string TotalPoint { get; set; }
        public string NameProduct { get; set; }
        public string NameMember { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MemberCode { get; set; }
        public string NameCollection { get; set; }
        public DateTime? CreateOrder { get; set; }
        public string zaloId { get; set; }

        public Guid CheckMemberId { get; set; }
        public Guid CheckProductItemId { get; set; }
        public double CheckPrice { get; set; }
        public double CheckPoint { get; set; } 
        public string Receiver { get; set; }
        public string PhoneNo { get; set; }
        
        public string ChangedUserId { get; set; }
        public List<MemberViewModel> MemberViewModels { get; set; }
        public List<ProductCollectionItemViewModel> CollectionItems { get; set; }
        public  MemberSearchView MemberSearchViewModel { get; set; }

    }
}
