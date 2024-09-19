using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Commons
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
