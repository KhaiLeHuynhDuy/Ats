using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models
{
    public class SelectedItemViewModel<T>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SelectedId { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
