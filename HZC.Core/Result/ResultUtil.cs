using System.Collections.Generic;

namespace HZC.Core
{
    public class ResultUtil
    {
        #region 操作成功
        public static Result Success(string msg = "")
        {
            return new Result { Code = 200, Message = msg };
        }

        public static Result<T> Success<T>(T body, string msg = "")
        {
            return new Result<T>(200, body, msg);
        }
        #endregion

        #region 操作失败
        public static Result Fail(string msg = "操作失败")
        {
            return new Result { Code = 201, Message = msg };
        }

        public static Result<T> Fail<T>(T body, string msg = "操作失败")
        {
            return new Result<T>(201, body, msg);
        }
        #endregion

        #region 验证失败
        public static Result AuthFail(string msg = "验证失败")
        {
            return new Result { Code = 204, Message = msg };
        }

        public static Result<T> AuthFail<T>(T body, string msg = "验证失败")
        {
            return new Result<T>(204, body, msg);
        }
        #endregion

        #region 列表
        public static ListResult<T> List<T>(IEnumerable<T> body, int code = 200, string message = "")
        {
            return new ListResult<T>
            {
                Code = code,
                Message = message,
                Body = body
            };
        }

        public static PageListResult<T> PageList<T>(PageList<T> body, int code = 200, string message = "")
        {
            return new PageListResult<T>
            {
                Code = code,
                Message = message,
                Body = body.Body,
                PageIndex = body.PageIndex,
                PageSize = body.PageSize,
                RecordCount = body.RecordCount
            };
        }
        #endregion
    }
}
