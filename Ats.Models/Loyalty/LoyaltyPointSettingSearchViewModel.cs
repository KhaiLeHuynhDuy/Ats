using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
    public class LoyaltyPointSettingSearchViewModel : BaseSearchViewModel
    {

        public List<LoyaltyPointSettingViewModel> LoyaltyPointSettings { get; set; }
        public LoyaltyPointSettingViewModel LoyaltyPointSetting { get; set; }

    }
}
