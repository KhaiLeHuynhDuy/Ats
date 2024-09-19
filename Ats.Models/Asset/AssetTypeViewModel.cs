using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetTypeViewModel
    {
        public AssetTypeViewModel()
        {
            AssetProperties = new HashSet<AssetPropertyViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AssetPropertyViewModel> AssetProperties { get; set; }
        public AssetPropertyViewModel AssetProperty { get; set; }
    }
}
