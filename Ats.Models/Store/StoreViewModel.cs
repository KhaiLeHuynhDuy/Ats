using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Store
{
    public class StoreViewModel
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public bool StoreCodeAutomatically { get; set; }

        public string StoreName { get; set; }
        public Guid? TeamId { get; set; }
        public string TeamName {get; set;}
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Desc { get; set; }
        public Boolean Active { get; set; } 
    }
}
