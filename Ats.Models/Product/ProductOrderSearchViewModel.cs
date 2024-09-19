using Ats.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductOrderSearchViewModel : BaseSearchViewModel
    {
        public ProductOrderSearchViewModel()
        {
            CheckBoxChangeStatus = new List<DisplayItem>();
        }
        public IList<DisplayItem> CheckBoxChangeStatus { get; set; }

        public List<ProductOrderViewModel> ProductOrders { get; set; } 
        public ProductOrderViewModel ProductOrder { get; set; }


        public List<ProductCollectionItemViewModel> CollectionItems { get; set; }
        public ProductCollectionItemViewModel ProductCollectionItem { get; set; }
        public ProductCollectionItemSearchViewModel CollectionItemSearchViewModel { get; set; }

        public List<MemberViewModel> MemberViewModels { get; set; }
        public MemberViewModel MemberViewModel { get; set; }
        public Guid CheckMemberId { get; set; }
        public Guid CheckItemID { get; set; }
        public Guid CheckOrderID { get; set; }
        public Guid IdMemberOld { get; set; }
        public Guid IdItemOld { get; set; }
        public int Status { get; set; }

       
    }
}
