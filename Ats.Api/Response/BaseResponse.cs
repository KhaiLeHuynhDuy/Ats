using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Api.Response
{
    public class BaseResponse<T> where T: class
    {
        public BaseResponse()
        {
            StatusCode = 200;
            Success = false;
            Message = "Error";
           
        }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
