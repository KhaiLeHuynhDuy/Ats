using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.OriginalFile
{
    public class OriginalFileSearchViewModel : BaseSearchViewModel
    {
        public List<OriginalFileViewModel> OriginalFiles { get; set; }
        public OriginalFileViewModel OriginalFile { get; set; }
    }
}
