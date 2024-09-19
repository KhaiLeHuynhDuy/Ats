namespace Ats.Models
{
    public class BaseSearchViewModel
    {
        public int? OrderBy { get; set; }
        public string Keyword { get; set; }

        public PagerViewModel Pager { get; set; }
        public PagerTowViewModel PagerTow { get; set; }
        public PagerThreeViewModel PagerThree { get; set; }

        public PagerProductPointsScanReportViewModel PagerProductPointsScan { get; set; }

        public PagerNewMemberReportsViewModel PagerNewMemberReports{ get; set; }

        public PagerRedemptionReportsViewModel PagerRedemptionReports { get; set; }


    }
}
