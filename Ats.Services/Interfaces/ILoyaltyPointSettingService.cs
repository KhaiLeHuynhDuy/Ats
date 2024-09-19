using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Loyalty;
using Ats.Models.MemberWallet;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface ILoyaltyPointSettingService
    {
        List<LoyaltyPointSettingViewModel> SearchLoyaltyPointSetting(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        LoyaltyPointSettingViewModel GetLoyaltyPointSetting(int id);
        void CreateLoyaltyPointSetting(LoyaltyPointSettingViewModel model);
        void UpdateLoyaltyPointSetting(LoyaltyPointSettingViewModel model);
        void DeleteLoyaltyPointSetting(int id);
        List<LoyaltyPointSettingViewModel> GetLoyaltyPointSettings();
    }
}
