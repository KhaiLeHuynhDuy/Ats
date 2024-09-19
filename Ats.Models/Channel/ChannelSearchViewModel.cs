using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Channel
{
    public class ChannelSearchViewModel : BaseSearchViewModel
    {
        public List<ChannelViewModel> Channels { get; set; }
        public ChannelViewModel Channel { get; set; }
    }

}
