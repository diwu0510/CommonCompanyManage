using System.Collections.Generic;

namespace HZC.Core
{
    public class PageList<T>
    {
        public int RecordCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<T> Body { get; set; }

        public PageList()
        { }

        public PageList(List<T> body, int recordCount, int pageIndex, int pageSize)
        {
            Body = body;
            RecordCount = recordCount;
            PageIndex = pageIndex;
            PageSize = PageSize;
        }
    }

    public class PageList : PageList<dynamic>
    { }
}
