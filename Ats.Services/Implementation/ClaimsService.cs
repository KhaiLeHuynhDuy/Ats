using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Commons;
using Ats.Domain;
using Ats.Models.Claims;
using Ats.Models.Loyalty;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;
namespace Ats.Services.Implementation
{
   public class ClaimsService : BaseService, IClaimsService
    {
        private IConfiguration _config;
        public ClaimsService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger ,config)
        { 
            _config = config;
        }
        public List<ClaimsViewModel> GetListClaims()
        {

            _logger.LogDebug($"Get Get List Claims");
            var claims = UnitOfWork.ClaimsReprository.GetAll().Select(x => new ClaimsViewModel()
            {
                Id = x.Id,
                ClaimValue = x.ClaimValue,
                ClaimType = x.ClaimType,

            }).ToList();

            return claims;
        }

        public bool getNoti()
        {
            var member = UnitOfWork.MemberRepo.GetAll().Count();
            var key = UnitOfWork.ClaimsReprository.GetAll().Where(x => x.ClaimType.Equals(Constants.Claimskey.SYS_CLAIMS_KEY)).FirstOrDefault().ClaimValue;
            if(member >= (Int32.Parse(key) * 0.85))
            {
                return false;
            }
            return true;
        }
    }
}
