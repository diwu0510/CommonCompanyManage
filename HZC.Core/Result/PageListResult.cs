using System.Collections.Generic;

namespace HZC.Core
{
    public class PageListResult<T> : Result
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int RecordCount { get; set; }

        public IEnumerable<T> Body { get; set; }

        public PageListResult()
        { }

        public PageListResult(int code, PageList<T> pageList, string message = "")
        {
            Code = code;
            Message = message;
            PageSize = pageList.PageSize;
            PageIndex = pageList.PageIndex;
            RecordCount = pageList.RecordCount;
            Body = pageList.Body;
        }

        public PageListResult(int code, IEnumerable<T> body, int pageIndex, int pageSize, int recordCount, string message = "")
        {
            Code = code;
            Body = body;
            PageSize = pageSize;
            PageIndex = pageIndex;
            RecordCount = recordCount;
            Message = message;
        }
    }
}
