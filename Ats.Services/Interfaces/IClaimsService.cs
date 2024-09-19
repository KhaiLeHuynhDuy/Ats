using Ats.Models.Claims;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
   public interface IClaimsService
    {
        List<ClaimsViewModel> GetListClaims();

        bool getNoti();

    }
}
