using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Store.Models;

namespace Ats.Models.Product
{
   public class ProductCollectionViewModel
    {
        public ProductCollectionViewModel()
        {
            Items = new List<DisplayItem>();
        }


        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public string? EffectiveDateBegins { get; set; }
        public string? EffectiveDateEnds { get; set; }

        public IList<DisplayItem> Items { get; set; }


    }
}
