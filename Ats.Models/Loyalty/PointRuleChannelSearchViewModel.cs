using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleChannelSearchViewModel : BaseSearchViewModel
    {
        public List<PointRuleChannelViewModel> PointRuleChannels { get; set; }
        public PointRuleChannelViewModel PointRuleChannel { get; set; }
    }
}
