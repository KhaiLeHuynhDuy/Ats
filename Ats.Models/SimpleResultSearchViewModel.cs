using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models
{
    public class SimpleResultSearchViewModel : BaseSearchViewModel
    {
        public List<SimpleResponseViewModel> Result { get; set; }
        public SimpleResponseViewModel Data { get; set; }
    }
}
