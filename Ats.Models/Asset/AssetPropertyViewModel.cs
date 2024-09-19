using Ats.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetPropertyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public string Description { get; set; }
        public DATA_TYPE DataType { get; set; }
        public bool IsActive { get; set; }
    }
}
