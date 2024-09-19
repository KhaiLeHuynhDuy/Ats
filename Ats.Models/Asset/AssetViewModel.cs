using Ats.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Asset
{
    public class AssetViewModel : BaseSearchViewModel
    {
        public AssetViewModel()
        {
            AssetAttributes = new HashSet<AssetAttributeModel>();
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public Guid? LoanId { get; set; }
        public double? LoanAmount { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public AssetAttributeModel AssetAttribute { get; set; }
        public ICollection<AssetAttributeModel> AssetAttributes { get; set; }
    }
}
