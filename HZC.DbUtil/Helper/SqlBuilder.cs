using System;
using System.Linq;
using HZC.Core;

namespace HZC.DbUtil
{
    public class SqlBuilder
    {
        public static string GetUpdateColumns(KeyValuePairs kvs)
        {
            return string.Join(",", kvs.Where(kv => kv.Key != "Id").Select(kv => $"{kv.Key}=@{kv.Key}"));
        }

        public static string GetSelect(string table, string cols, string where, string orderBy)
        {
            return $"SELECT [{(string.IsNullOrWhiteSpace(cols) ? "*" : cols)}] FROM [{table}] WHERE {(string.IsNullOrWhiteSpace(where) ? "1=1" : where)}{(string.IsNullOrWhiteSpace(orderBy)? "" : " ORDER BY {orderBy}")}";
        }

        public static string GetPaginSelect(string table, string cols, string where, string orderBy, int pageIndex, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentNullException(nameof(table), "数据表不能为空");
            if (string.IsNullOrWhiteSpace(orderBy)) throw new ArgumentNullException(nameof(orderBy), "排序字段不能为空");
            
            cols = string.IsNullOrWhiteSpace(cols) ? "*" : cols;
            where = string.IsNullOrWhiteSpace(where) ? "1=1" : where;
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            var sql = pageIndex == 1
                ? $"SELECT TOP {pageSize} {cols} FROM [{table}] WHERE {@where} ORDER BY {orderBy};SELECT @RecordCount=COUNT(0) FROM {table} WHERE {@where}"
                : $@"WITH PAGEDDATA AS
					    (
						    SELECT TOP 100 PERCENT {cols}, ROW_NUMBER() OVER (ORDER BY {orderBy}) AS FLUENTDATA_ROWNUMBER
						    FROM {table}
					    )
					    SELECT *
					    FROM PAGEDDATA
					    WHERE FLUENTDATA_ROWNUMBER BETWEEN {(pageIndex - 1) * pageSize} AND {pageIndex * pageSize};
                        SELECT @RecordCount=COUNT(0) FROM {table} WHERE {@where}";
            return sql;
        }
    }
}
