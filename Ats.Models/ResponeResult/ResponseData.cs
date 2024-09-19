using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.ResponeResult
{
    public class ResponseData
    {
        public bool Success { get; }
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public dynamic Data { get; set; }
        public ResponseData(bool success, int statusCode, string description, dynamic data)
        {
            Success = success;
            Description = description;
            StatusCode = statusCode;
            Data = data;
        }
    }
}
