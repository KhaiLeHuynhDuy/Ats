using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpnet.WebApp.Models
{
    public class SimpleCreationViewModel<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public int SelectedValue { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}