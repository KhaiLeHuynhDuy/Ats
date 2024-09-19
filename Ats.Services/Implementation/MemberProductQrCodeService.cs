using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Member;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ats.Services.Implementation
{
    public class MemberProductQrCodeService : BaseService, IMemberProductQrCodeService
    {
        private IConfiguration _config;

        public MemberProductQrCodeService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
           
        }

        public int GetStatisticsMemberProductQrCodes(DateTime dayb, DateTime daye)
        {
            //DateTime dateB = DateTime.Parse(dayb);
            //DateTime dateE = DateTime.Parse(daye);
            _logger.LogDebug($"GetAlls");
            var MemberProductQeCode = UnitOfWork.MemberProductQrCodeRepo.GetAll().Select(x => new MemberProductQrCodeViewModel()
            {
                Id = x.Id, 
                Date = x.AddedTimestamp
            }).Where(m => m.Date >= dayb && m.Date <= daye).Count();
            return MemberProductQeCode;
        }
        public int GetStatisticsMemberProductQrCodes()
        {
            var MemberProductQeCode = UnitOfWork.MemberProductQrCodeRepo.GetAll().Select(x => new MemberProductQrCodeViewModel()
            {
                Id = x.Id
            }).Count();
            return MemberProductQeCode;
        }
        public int GetStatisticsMemberProductQrCodesThisMonth(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        { 
            var MemberProductQeCode = UnitOfWork.MemberProductQrCodeRepo.GetAll().Select(x => new MemberProductQrCodeViewModel()
            {
                Id = x.Id,
                Date = x.AddedTimestamp
            }).Where(m => m.Date > firstDayOfMonth || m.Date == firstDayOfMonth && m.Date < lastDayOfMonth || m.Date == lastDayOfMonth).Count();
            return MemberProductQeCode;
        }

    }
}
