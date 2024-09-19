using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Loyalty;
using Ats.Models.MemberWallet;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface ILoyaltyPointTypeService
    {
        List<LoyaltyPointTypeViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        LoyaltyPointTypeViewModel GetLoyaltyPointType(int id);
        void CreateLoyaltyPointType(LoyaltyPointTypeViewModel model);
        void UpdateLoyaltyPointType(LoyaltyPointTypeViewModel model);
        void DeleteLoyaltyPointType(int id);
        List<LoyaltyPointTypeViewModel> GetLoyaltyPointTypes();
    }
}
