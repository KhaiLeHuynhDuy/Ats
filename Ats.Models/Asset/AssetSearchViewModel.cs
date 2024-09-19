using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetSearchViewModel : BaseSearchViewModel
    {
        public List<AssetViewModel> Assets { get; set; }
        public AssetViewModel Asset { get; set; }
    }
}
