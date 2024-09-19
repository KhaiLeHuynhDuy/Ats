using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    public enum PRODUCT_ORDER_STATUS {
        [Display(Name = "Đang Duyệt")]
        NEW = 1,
        [Display(Name = "Đang Giao")]
        ON_DELIVERY = 2,
        [Display(Name = "Hoàn Thành")]
        COMPLETE = 3,
        [Display(Name = "Đã Hủy")]
        CANCEL = 4
    };

    [Table("productorders")]
    public class ProductOrder : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductItemId { get; set; }
        [ForeignKey("ProductItemId")]
        public virtual ProductCollectionItem ProductItem { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member.Models.Member Member { get; set; }

        public string Receiver { get; set; }
        public string PhoneNo { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string Remark { get; set; }
        public PRODUCT_ORDER_STATUS OrderStatus { get; set; }
        public string ShippingAddress { get; set; }
    }
}
