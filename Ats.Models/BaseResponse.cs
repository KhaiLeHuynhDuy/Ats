using System.Collections.Generic;

namespace Ats.Models
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }

        public List<string> Errors { get; set; }

        public T Data { get; set; }
        public Pager Pager { get; set; }
    }

    public class Pager
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}