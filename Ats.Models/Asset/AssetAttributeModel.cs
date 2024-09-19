using Ats.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetAttributeModel
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int AssetPropertyId { get; set; }
        public string AssetPropertyName { get; set; }
        public string AssetPropertyShortName { get; set; }
        public DATA_TYPE DataType { get; set; }
        public string Value { get; set; }
    }
}
