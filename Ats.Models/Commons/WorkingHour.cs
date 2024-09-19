using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Commons
{
    public class WorkingHour
    {
        public int SatTime { get; set; }
        public int SunTime { get; set; }
        public int TimePerDay { get; set; }
        public int TimePerShift { get; set; }
        public int TimeStart { get; set; }
        public int TimeEnd { get; set; }
        public int RestHour { get; set; }
        public int RestTime { get; set; }
        public int WithdrawDay { get; set; }
    }
}
