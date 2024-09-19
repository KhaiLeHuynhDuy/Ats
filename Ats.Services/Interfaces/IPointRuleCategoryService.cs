using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.Loyalty;

namespace Ats.Services.Interfaces
{
   public interface IPointRuleCategoryService
    {
        List<PointRuleCategoryViewModel> Gets();
    }
}
