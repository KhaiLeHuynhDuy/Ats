using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Channel
{
    public class ChannelViewModel
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public string Desc { get; set; }
        public Boolean Active { get; set; } 
    }
}
