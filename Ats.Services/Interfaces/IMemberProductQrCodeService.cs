using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IMemberProductQrCodeService : IBaseService
    {
        int GetStatisticsMemberProductQrCodes(DateTime dayb, DateTime daye);
        int GetStatisticsMemberProductQrCodes();
        int GetStatisticsMemberProductQrCodesThisMonth(DateTime firstDayOfMonth, DateTime lastDayOfMonth);

    }
}
