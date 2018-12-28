using Dapper;
using HZC.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HZC.DbUtil
{
    public static class DapperExtensionT
    {
        #region 创建
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <typeparam name="T">BaseEntity</typeparam>
        /// <param name="conn"></param>
        /// <param name="entity">要创建的实体</param>
        /// <param name="trans">事务</param>
        /// <returns>实体的Id</returns>
        public static int Create<T>(this SqlConnection conn, T entity, SqlTransaction trans = null) where T : BaseEntity
        {
            var sql = MyContainer.Get(typeof(T)).InsertSqlStatement;
            return conn.ExecuteScalar<int>(sql, entity, trans);
        }

        public static int Create<T>(this SqlConnection conn, List<T> entities, SqlTransaction trans = null) where T : BaseEntity
        {
            var sql = MyContainer.Get(typeof(T)).InsertSqlStatement;
            return conn.Execute(sql, entities, trans);
        }
        #endregion

        #region 更新
        public static int Update<T>(this SqlConnection connection, T entity, SqlTransaction trans = null) where T : BaseEntity
        {
            var sql = MyContainer.Get(typeof(T)).UpdateSqlStatement;
            return connection.Execute(sql, entity, trans);
        }

        public static int Update<T>(this SqlConnection connection, List<T> entities, SqlTransaction trans = null) where T : BaseEntity
        {
            var sql = MyContainer.Get(typeof(T)).UpdateSqlStatement;
            return connection.Execute(sql, entities, trans);
        }

        public static int Update<T>(this SqlConnection connection, int id, KeyValuePairs kvs, SqlTransaction tran = null) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Update(table, kvs, id);
        }

        public static int Update<T>(this SqlConnection connection, int[] ids, KeyValuePairs kvs, SqlTransaction tran = null) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Update(table, kvs, ids);
        }

        public static int UpdateInclude<T>(this SqlConnection connection, string[] columns, T entity, SqlTransaction trans = null) where T : BaseEntity
        {
            var model = MyContainer.Get(typeof(T));
            var table = model.Table;

            var allColumns = model.Properties.Where(p => p.PropertyName != "Id").Select(p => p.PropertyName);
            var cols = columns.Intersect(allColumns).ToList();

            if (!cols.Any()) throw new ArgumentNullException("未包含有效要更新的列");

            var sql = $"UPDATE [{table}] SET {string.Join(",", cols.Select(c => $"{c}=@{c}"))} WHERE Id=@Id";
            return connection.Execute(sql, entity, trans);
        }

        public static int UpdateExclude<T>(this SqlConnection connection, string[] columns, T entity, SqlTransaction trans = null) where T : BaseEntity
        {
            var model = MyContainer.Get(typeof(T));
            var table = model.Table;

            var allColumns = model.Properties.Where(p => p.PropertyName != "Id").Select(p => p.PropertyName);
            var cols = allColumns.Except(columns).ToList();

            if (!cols.Any()) throw new ArgumentNullException("未包含有效要更新的列");

            var sql = $"UPDATE [{table}] SET {string.Join(",", cols.Select(c => $"{c}=@{c}"))} WHERE Id=@Id";
            return connection.Execute(sql, entity, trans);
        }

        public static int UpdateInclude<T>(this SqlConnection connection, string[] columns, List<T> entities, SqlTransaction trans = null) where T : BaseEntity
        {
            var model = MyContainer.Get(typeof(T));
            var table = model.Table;

            var allColumns = model.Properties.Where(p => p.PropertyName != "Id").Select(p => p.PropertyName);
            var cols = columns.Intersect(allColumns).ToList();

            if (!cols.Any()) throw new ArgumentNullException("未包含有效要更新的列");

            var sql = $"UPDATE [{table}] SET {string.Join(",", cols.Select(c => $"{c}=@{c}"))} WHERE Id=@Id";
            return connection.Execute(sql, entities, trans);
        }

        public static int UpdateExclude<T>(this SqlConnection connection, string[] columns, List<T> entities, SqlTransaction trans = null) where T : BaseEntity
        {
            var model = MyContainer.Get(typeof(T));
            var table = model.Table;

            var allColumns = model.Properties.Where(p => p.PropertyName != "Id").Select(p => p.PropertyName);
            var cols = allColumns.Except(columns).ToList();

            if (!cols.Any()) throw new ArgumentNullException("未包含有效要更新的列");

            var sql = $"UPDATE [{table}] SET {string.Join(",", cols.Select(c => $"{c}=@{c}"))} WHERE Id=@Id";
            return connection.Execute(sql, entities, trans);
        }

        public static int Update<T>(this SqlConnection connection, KeyValuePairs kvs, MySearchUtil util, SqlTransaction trans = null)
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Update(table, kvs, util, trans);
        }
        #endregion

        #region 删除
        public static int Delete<T>(this SqlConnection connection, T entity, SqlTransaction trans = null) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            var sql = $"DELETE [{table}] WHERE Id=@Id";
            return connection.Execute(sql, new { entity.Id }, trans);
        }

        public static int Delete<T>(this SqlConnection connection, List<T> entities, SqlTransaction trans = null) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            var sql = $"DELETE [{table}] WHERE Id IN @Ids";
            return connection.Execute(sql, new { Ids = entities.Select(e => e.Id) }, trans);
        }

        public static int Delete<T>(this SqlConnection connection, int id, SqlTransaction trans = null) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Delete(table, id, trans);
        }

        public static int Delete<T>(this SqlConnection connection, int[] ids) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Delete(table, ids);
        }

        public static int Delete<T>(this SqlConnection connection, MySearchUtil util = null)
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Delete(table, util);
        }
        #endregion

        #region 移除
        public static int Remove<T>(this SqlConnection connection, T entity, SqlTransaction trans = null)
            where T : BaseEntity, IRemoveAble
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, entity.Id, trans);
        }

        public static int Remove<T>(this SqlConnection connection, List<T> entities, SqlTransaction trans = null)
            where T : BaseEntity, IRemoveAble
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, entities.Select(e => e.Id).ToArray(), trans);
        }

        public static int Remove<T>(this SqlConnection connection, int id, SqlTransaction trans = null)
            where T : BaseEntity, IRemoveAble
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, id, trans);
        }

        public static int Remove<T>(this SqlConnection connection, int[] ids, SqlTransaction trans = null)
            where T : BaseEntity, IRemoveAble
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, ids, trans);
        }

        public static int Remove<T>(this SqlConnection connection, MySearchUtil util, SqlTransaction trans = null)
            where T : BaseEntity, IRemoveAble
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, util, trans);
        }
        #endregion

        #region 移除，记录操作人
        public static int Remove<T>(this SqlConnection connection, T entity, BaseAppUser user, SqlTransaction trans = null)
            where T : TraceEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, entity.Id, user, trans);
        }

        public static int Remove<T>(this SqlConnection connection, IEnumerable<T> entities, BaseAppUser user, SqlTransaction trans = null)
            where T : TraceEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, entities.Select(e => e.Id).ToArray(), user, trans);
        }

        public static int Remove<T>(this SqlConnection connection, BaseAppUser user, MySearchUtil util, SqlTransaction trans = null)
            where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, user, util, trans);
        }

        public static int Remove<T>(this SqlConnection connection, int id, BaseAppUser user, SqlTransaction trans = null)
            where T : TraceEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, id, user, trans);
        }

        public static int Remove<T>(this SqlConnection connection, int[] ids, BaseAppUser user, SqlTransaction trans = null)
            where T : TraceEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Remove(table, ids, user, trans);
        }
        #endregion

        #region 加载实体
        public static T Load<T>(this SqlConnection connection, int id, SqlTransaction trans = null, string table = "")
        {
            table = string.IsNullOrWhiteSpace(table) ? MyContainer.Get(typeof(T)).Table : table;
            var sql = $"SELECT TOP 1 * FROM [{table}] WHERE Id=@Id";
            return connection.QueryFirst(sql, new { Id = id }, trans);
        }

        public static T Load<T>(this SqlConnection connection, MySearchUtil util = null, SqlTransaction trans = null, string table = "")
        {
            util = util ?? new MySearchUtil();
            table = string.IsNullOrWhiteSpace(table) ? MyContainer.Get(typeof(T)).Table : table;

            var where = util.GetWhere();
            var orderBy = util.GetOrderBy();
            var param = util.GetParam();

            var sql = $"SELECT TOP 1 * FROM [{table}] WHERE {where} ORDER BY {orderBy}";
            return connection.QueryFirst(sql, param, trans);
        }
        #endregion

        #region 加载列表
        public static List<T> Fetch<T>(this SqlConnection connection, string table, MySearchUtil util = null, string cols = "*", int top = 0)
        {
            cols = string.IsNullOrWhiteSpace(cols) ? "*" : cols;
            util = util ?? new MySearchUtil();

            var where = util.GetWhere();
            var param = util.GetParam();
            var orderBy = util.GetOrderBy();

            var topStr = top > 0 ? $" TOP {top}" : "";

            var sql = $"SELECT{topStr} {cols} FROM [{table}] WHERE {where} ORDER BY {orderBy}";
            return connection.Query<T>(sql, param).ToList();
        }

        public static List<T> Fetch<T>(this SqlConnection connection, MySearchUtil util = null, string cols = "*", int top = 0)
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Fetch<T>(table, util, cols, top);
        }
        #endregion

        #region 分页列表
        public static PageList<T> PageList<T>(this SqlConnection connection,
            string table, int pageIndex, int pageSize, MySearchUtil util, string cols = "*")
        {
            if (util == null) throw new ArgumentNullException(nameof(util), "查询参数不能为空");

            var where = util.GetWhere();
            var param = util.GetParam(true);
            var orderBy = util.GetOrderBy();

            var sql = SqlBuilder.GetPaginSelect(table, cols, where, orderBy, pageIndex, pageSize);
            var body = connection.Query<T>(sql, param);
            return new PageList<T>
            {
                Body = body.ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = param.Get<int>("RecordCount")
            };
        }

        public static PageList<T> PageList<T>(this SqlConnection connection, int pageIndex, int pageSize, MySearchUtil util, string cols = "*")
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.PageList<T>(table, pageIndex, pageSize, util, cols);
        }
        #endregion

        #region 数量

        public static int Count<T>(this SqlConnection connection, MySearchUtil util = null, SqlTransaction trans = null) where T : BaseEntity
        {
            var table = MyContainer.Get(typeof(T)).Table;
            return connection.Count(table, util, trans);
        }
        #endregion
    }
}
