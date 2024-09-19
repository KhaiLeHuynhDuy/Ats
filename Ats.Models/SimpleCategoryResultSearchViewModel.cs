using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models
{
    public class SimpleCategoryResultSearchViewModel<T> : BaseSearchViewModel
    {
        public List<SimpleCategoryResponseViewModel<T>> Result { get; set; }
        public SimpleCategoryResponseViewModel<T> Data { get; set; }
        public List<SimpleResponseViewModel> CategoryItems { get; set; }
    }
}
