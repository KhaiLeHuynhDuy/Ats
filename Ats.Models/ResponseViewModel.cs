using System.Collections.Generic;

namespace Ats.Models
{
    public class ResponseViewModel<T>
    {
        public string Title { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}