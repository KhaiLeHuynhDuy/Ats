using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Loyalty.Models;
using Ats.Models;
using Ats.Models.LoyaltyTier;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface ILoyaltyTierService
    {
        List<LoyaltyTierViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        LoyaltyTierViewModel GetLoyaltyTier(int id);
        void CreateLoyaltyTier(LoyaltyTierViewModel model);
        void UpdateLoyaltyTier(LoyaltyTierViewModel model);
        void DeleteLoyaltyTier(int id);
        List<LoyaltyTierViewModel> GetLoyaltyTiers();
     
    }
}
