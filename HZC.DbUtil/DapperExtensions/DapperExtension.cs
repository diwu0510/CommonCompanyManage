using Dapper;
using HZC.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HZC.DbUtil
{
    public static class DapperExtension
    {
        #region 更新
        /// <summary>
        /// 更新指定表的指定字段
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="table"></param>
        /// <param name="kvs"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Update(this SqlConnection connection, string table, KeyValuePairs kvs, int id)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentNullException(nameof(table), "数据表不能为空");
            if (kvs == null || kvs.Any(kv => kv.Key == "Id")) throw new ArgumentNullException(nameof(kvs), "要更新的列不能为空，且不能包含Id字段");

            var columns = new List<string>();
            var param = new DynamicParameters();
            param.Add("Id", id);
            foreach (var kv in kvs.Where(kv => kv.Key != "Id"))
            {
                columns.Add($"{kv.Key}=@{kv.Key}");
                param.Add(kv.Key, kv.Value);
            }
            var sql = $"UPDATE [{table}] SET {string.Join(",", columns)} WHERE Id=@Id";
            return connection.Execute(sql, param);
        }

        public static int Update(this SqlConnection connection, string table, KeyValuePairs kvs, int[] ids)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentNullException(nameof(table), "数据表不能为空");
            if (kvs == null || kvs.Any(kv => kv.Key == "Id")) throw new ArgumentNullException(nameof(kvs), "要更新的列不能为空，并且不能包含Id列");
            if (ids == null || !ids.Any()) throw new ArgumentNullException(nameof(ids), "ID数组不能为空");

            var columns = new List<string>();
            var param = new DynamicParameters();
            param.Add("Ids", ids);
            foreach (var kv in kvs.Where(kv => kv.Key != "Id"))
            {
                columns.Add($"{kv.Key}=@{kv.Key}");
                param.Add(kv.Key, kv.Value);
            }
            var sql = $"UPDATE [{table}] SET {string.Join(",", columns)} WHERE Id in @Ids";
            return connection.Execute(sql, param);
        }

        public static int Update(this SqlConnection connection, string table, KeyValuePairs kvs, MySearchUtil util = null, SqlTransaction trans = null)
        {
            if (string.IsNullOrWhiteSpace(table)) throw new ArgumentNullException(nameof(table), "数据表不能为空");
            if (kvs == null || kvs.Any(kv => kv.Key == "Id")) throw new ArgumentNullException(nameof(kvs), "要更新的列不能为空，并且不能包含Id列");

            util = util ?? new MySearchUtil();

            var cols = string.Join(",", kvs.Where(kv => kv.Key != "Id").Select(kv => $"{kv.Key}=@{kv.Key}"));
            var where = util.GetWhere();
            var param = util.GetParam();

            var sql = $"UPDATE {table} SET {cols} WHERE {where}";
            return connection.Execute(sql, param, trans);
        }
        #endregion

        #region 删除
        public static int Delete(this SqlConnection connection, string table, int id, SqlTransaction trans = null)
        {
            var sql = $"DELETE [{table}] WHERE Id=@Id";
            return connection.Execute(sql, new { Id = id }, trans);
        }

        public static int Delete(this SqlConnection connection, string table, int[] ids, SqlTransaction trans = null)
        {
            var sql = $"DELETE [{table}] WHERE Id IN @Ids";
            return connection.Execute(sql, new { Ids = ids }, trans);
        }

        public static int Delete(this SqlConnection connection, string table, MySearchUtil util, SqlTransaction trans = null)
        {
            if (util == null) throw new ArgumentNullException(nameof(util));

            var where = util.GetWhere();
            var param = util.GetParam();

            var sql = $"DELETE [{table}] WHERE {where}";
            return connection.Execute(sql, param, trans);
        }
        #endregion

        #region 移除
        public static int Remove(this SqlConnection connection, string table, int id, SqlTransaction trans = null)
        {
            var sql = $"UPDATE [{table}] SET IsDel=1 WHERE Id=@Id";
            return connection.Execute(sql, new { Id = id });
        }

        public static int Remove(this SqlConnection connection, string table, int[] ids, SqlTransaction trans = null)
        {
            var sql = $"UPDATE [{table}] SET IsDel=1 WHERE Id IN @Ids";
            return connection.Execute(sql, new { Ids = ids });
        }

        public static int Remove(this SqlConnection connection, string table, MySearchUtil util, SqlTransaction trans = null)
        {
            if (util == null) throw new ArgumentNullException(nameof(util));

            var where = util.GetWhere();
            var param = util.GetParam();
            var sql = $"UPDATE [{table}] SET IsDel=1 WHERE {where}";
            return connection.Execute(sql, param, trans);
        }

        public static int Remove(this SqlConnection connection, string table, int id, BaseAppUser user, SqlTransaction trans = null)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var sql = $"UPDATE [{table}] SET IsDel=1,UpdateAt=GETDATE(),Updator=@Updator WHERE Id=@Id";
            return connection.Execute(sql, new { Id = id, Updator = user.Name });
        }

        public static int Remove(this SqlConnection connection, string table, int[] ids, BaseAppUser user, SqlTransaction trans = null)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var sql = $"UPDATE [{table}] SET IsDel=1,UpdateAt=GETDATE(),Updator=@Updator WHERE Id IN @Ids";
            return connection.Execute(sql, new { Ids = ids, Updator = user.Name });
        }

        public static int Remove(this SqlConnection connection, string table, BaseAppUser user,
            MySearchUtil util, SqlTransaction trans = null)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (util == null) throw new ArgumentNullException(nameof(util));
            
            var where = util.GetWhere();
            var param = util.GetParam();
            param.Add("Updator", user.Name);
            var sql = $"UPDATE [{table}] SET IsDel=1,UpdateAt=GETDATE(),Updator=@Updator WHERE {where}";
            return connection.Execute(sql, param, trans);
        }
        #endregion

        #region 所有数据
        public static List<dynamic> Fetch(this SqlConnection connection, string table, MySearchUtil util = null, string cols = "*", int top = 0)
        {
            cols = string.IsNullOrWhiteSpace(cols) ? "*" : cols;
            util = util ?? new MySearchUtil();

            var where = util.GetWhere();
            var param = util.GetParam();
            var orderBy = util.GetOrderBy();

            var topStr = top > 0 ? $" TOP {top}" : "";

            var sql = $"SELECT{topStr} {cols} FROM [{table}] WHERE {where} ORDER BY {orderBy}";
            return connection.Query(sql, param).ToList();
        }
        #endregion

        #region 分页数据
        public static PageList PageList(this SqlConnection connection,
            string table, int pageIndex, int pageSize, MySearchUtil util, string cols = "*")
        {
            if (util == null) throw new ArgumentNullException(nameof(util), "查询参数不能为空，至少要提供一个排序");

            var where = util.GetWhere();
            var param = util.GetParam(true);
            var orderBy = util.GetOrderBy();

            var sql = SqlBuilder.GetPaginSelect(table, cols, where, orderBy, pageIndex, pageSize);
            var body = connection.Query(sql, param);
            return new PageList
            {
                Body = body.ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = param.Get<int>("RecordCount")
            };
        }
        #endregion

        #region 数量

        public static int Count(this SqlConnection connection, string table, MySearchUtil util = null, SqlTransaction trans = null)
        {
            util = util ?? new MySearchUtil();

            var where = util.GetWhere();
            var param = util.GetParam();

            var sql = $"SELECT COUNT(0) FROM [{table}] WHERE {where}";
            return connection.ExecuteScalar<int>(sql, param, trans);
        }
        #endregion
    }
}
