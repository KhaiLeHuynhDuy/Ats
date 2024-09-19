using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Unit
{
    public class UnitViewModel
    {
        public int Id { get; set; }
        public int? UnitId { get; set; }
        public string Name { get; set; }
        public string  Desc{ get; set; }
        public bool IsActive { get; set; }
    }
}
