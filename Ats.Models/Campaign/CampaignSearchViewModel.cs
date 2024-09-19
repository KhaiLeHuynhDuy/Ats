using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Campaign
{
   public class CampaignSearchViewModel : BaseSearchViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public CampaignViewModel Campaign { get; set; }

        public List<ScheduledViewModel> scheduleds { get; set; }
        public ScheduledViewModel scheduled { get; set; }

        public List<OnGoingViewModel> OnGoings { get; set; }
        public OnGoingViewModel onGoing { get; set; }

        public List<PastViewModel> pastViews { get; set; }
        public PastViewModel pastView { get; set; }
    }
}
