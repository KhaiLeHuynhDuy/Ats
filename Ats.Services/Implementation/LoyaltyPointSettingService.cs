using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.MemberWallet;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Domain.Loyalty.Models;
using Ats.Models.Loyalty;

namespace Ats.Services.Implementation
{
    public class LoyaltyPointSettingService : BaseService, ILoyaltyPointSettingService
    {
        private IConfiguration _config;
        private int pageSize;

        public LoyaltyPointSettingService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public void CreateLoyaltyPointSetting(LoyaltyPointSettingViewModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteLoyaltyPointSetting(int id)
        {
            throw new NotImplementedException();
        }

        public LoyaltyPointSettingViewModel GetLoyaltyPointSetting(int id)
        {
            throw new NotImplementedException();
        }

        public List<LoyaltyPointSettingViewModel> GetLoyaltyPointSettings()
        {
            throw new NotImplementedException();
        }

        public List<LoyaltyPointSettingViewModel> SearchLoyaltyPointSetting(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            throw new NotImplementedException();
        }

        public void UpdateLoyaltyPointSetting(LoyaltyPointSettingViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
