using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.JobTitle
{
    public class JobTitleSearchViewModel : BaseSearchViewModel
    {
        public List<JobTitleViewModel> JobTitles{ get; set; }
        public JobTitleViewModel JobTitle { get; set; }
    }
}
