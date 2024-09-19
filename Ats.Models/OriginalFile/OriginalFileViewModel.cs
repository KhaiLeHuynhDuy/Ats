using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.OriginalFile
{
    public class OriginalFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
