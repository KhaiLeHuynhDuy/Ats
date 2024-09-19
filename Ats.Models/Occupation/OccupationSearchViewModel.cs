using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Occupation
{
    public class OccupationSearchViewModel : BaseSearchViewModel
    {
        public List<OccupationViewModel> Occupations { get; set; }
        public OccupationViewModel Occupation { get; set; }
    }
}
