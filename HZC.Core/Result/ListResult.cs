using System.Collections.Generic;

namespace HZC.Core
{
    public class ListResult<T>
    {
        public int Code { get; set; }
        
        public string Message { get; set; }

        public IEnumerable<T> Body { get; set; }

        public ListResult()
        { }

        public ListResult(int code, IEnumerable<T> body, string message = "")
        {
            Code = code;
            Body = body;
            Message = message;
        }
    }
}
