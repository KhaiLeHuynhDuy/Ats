using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ats.Models
{
    public class PagerThreeViewModel
    {
        public PagerThreeViewModel(string action, int page, int size)
        {
            this.Action = action;
            this.Page = page;
            this.Size = size;
        }


        public string Action { get; set; }
        public string Search { get; set; }
        public string OrderField { get; set; }
        public bool OrderSort { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 30;
        public int DisplayPage { get; set; } = 5;
        public int TotalItem { get; set; }
        public int TotalItemInPage { get; set; }
    }
}