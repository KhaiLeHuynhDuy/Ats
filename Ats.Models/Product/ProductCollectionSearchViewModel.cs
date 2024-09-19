using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
   public class ProductCollectionSearchViewModel : BaseSearchViewModel
    {
        public List<ProductCollectionViewModel> ProductCollections { get; set; }
        public ProductCollectionViewModel ProductCollection { get; set; }

        public ProductCollectionItemViewModel ProductCollectionItem { get; set; }
        public List<ProductCollectionItemViewModel> ProductCollectionItems { get; set; }

        public List<ProductViewModel> Products  { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
