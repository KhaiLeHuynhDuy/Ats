using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetTypeSearchViewModel : BaseSearchViewModel
    {
        public List<AssetTypeViewModel> AssetTypes { get; set; }
        public AssetTypeViewModel AssetType { get; set; }
    }
}
