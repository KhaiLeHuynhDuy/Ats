using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models
{
    public class SimpleCategoryResponseViewModel<T>
    {
        public int Id { get; set; }
        public T CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
