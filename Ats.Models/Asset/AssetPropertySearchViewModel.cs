using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetPropertySearchViewModel : BaseSearchViewModel
    {
        public List<AssetPropertyViewModel> AssetProperties { get; set; }
        public AssetPropertyViewModel AssetProperty { get; set; }
    }
}
