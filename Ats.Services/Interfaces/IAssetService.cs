using Ats.Models.Asset;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IAssetService
    {
        #region Asset Type
        List<AssetTypeViewModel> SearchAssetType(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        AssetTypeViewModel GetAssetType(int id);
        void CreateAssetType(AssetTypeViewModel model);
        void UpdateAssetType(AssetTypeViewModel model);
        void DeleteAssetType(int id);
        #endregion

        #region Asset Property
        List<AssetPropertyViewModel> SearchAssetProperty(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<AssetPropertyViewModel> GetAssetProperties();
        AssetPropertyViewModel GetAssetProperty(int id);
        void CreateAssetProperty(AssetPropertyViewModel model);
        void UpdateAssetProperty(AssetPropertyViewModel model);
        void DeleteAssetProperty(int id);
        #endregion

        #region Asset
        List<AssetViewModel> SearchAsset(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        AssetViewModel GetAsset(int id);
        void CreateAsset(AssetViewModel model);
        void UpdateAsset(AssetViewModel model);
        void DeleteAsset(int id);
        #endregion

        #region Asset Attribute
        bool CheckExistAddAssetAttribute(int productId, int productPropertyId);
        void AddAssetAttribute(AssetAttributeModel model);
        AssetAttributeModel GetAssetAttribute(int id);
        void UpdateAssetAttribute(AssetAttributeModel model);
        void DeleteAssetAttribute(int id);
        #endregion
    }
}
