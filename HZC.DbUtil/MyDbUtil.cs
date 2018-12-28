using Dapper;
using HZC.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HZC.DbUtil
{
    public class MyDbUtil
    {
        private static IConfiguration _configuration;
        private readonly string _connectionString;

        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region 构造函数
        public MyDbUtil(string sectionName = "")
        {
            if (_configuration == null)
            {
                throw new Exception("数据库工具未初始化，要在Startup.cs中调用MyDbUtil.Init(Configuration);进行注册");
            }
            sectionName = string.IsNullOrWhiteSpace(sectionName) ? "DefaultConnectionString" : sectionName.Trim();
            _connectionString = _configuration.GetConnectionString(sectionName);
        }
        #endregion

        #region 获取数据库连接
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        #endregion

        #region 创建
        public int Create<T>(T entity) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Create(entity);
            }
        }

        public int Create<T>(List<T> entities) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Create(entities);
            }
        }
        #endregion

        #region 删除
        public int Delete(string table, int id)
        {
            using (var conn = GetConnection())
            {
                return conn.Delete(table, id);
            }
        }

        public int Delete(string table, int[] ids)
        {
            using (var conn = GetConnection())
            {
                return conn.Delete(table, ids);
            }
        }

        public int Delete(string table, MySearchUtil searchUtil)
        {
            using (var conn = GetConnection())
            {
                return conn.Delete(table, searchUtil);
            }
        }

        public int Delete<T>(T entity) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Delete(entity);
            }
        }

        public int Delete<T>(List<T> entities) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Delete(entities);
            }
        }

        public int Delete<T>(int id) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Delete<T>(id);
            }
        }

        public int Delete<T>(int[] ids) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Delete<T>(ids);
            }
        }

        public int Delete<T>(MySearchUtil util) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Delete<T>(util);
            }
        }
        #endregion

        #region 加载所有
        public List<T> Fetch<T>(MySearchUtil util = null, string cols = "*", int top = 0)
        {
            using (var conn = GetConnection())
            {
                return conn.Fetch<T>(util, cols, top);
            }
        }

        public List<T> Fetch<T>(string table, MySearchUtil util = null, string cols = "*", int top = 0)
        {
            using (var conn = GetConnection())
            {
                return conn.Fetch<T>(table, util, cols, top);
            }
        }

        public List<T> Fetch<T>(string sql, object param)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, param).ToList();
            }
        }

        public List<dynamic> Fetch(string sql, object param)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, param).ToList();
            }
        }

        public List<dynamic> Fetch(string table, MySearchUtil util = null, string cols = "*", int top = 0)
        {
            using (var conn = GetConnection())
            {
                return conn.Fetch(table, util, cols, top);
            }
        }
        #endregion

        #region 加载实体
        public T Load<T>(int id, string table = "")
        {
            using (var conn = GetConnection())
            {
                return conn.Load<T>(id, null, table);
            }
        }

        public T Load<T>(MySearchUtil searchUtil, string table = "")
        {
            using (var conn = GetConnection())
            {
                return conn.Load<T>(searchUtil, null, table);
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP1"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>  
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, TP1>(string sql, Func<T, TP1, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP1"></typeparam>
        /// <typeparam name="TP2"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, TP1, TP2>(string sql, Func<T, TP1, TP2, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP1"></typeparam>
        /// <typeparam name="TP2"></typeparam>
        /// <typeparam name="TP3"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, TP1, TP2, TP3>(string sql, Func<T, TP1, TP2, TP3, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP1"></typeparam>
        /// <typeparam name="TP2"></typeparam>
        /// <typeparam name="TP3"></typeparam>
        /// <typeparam name="TP4"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, TP1, TP2, TP3, TP4>(string sql, Func<T, TP1, TP2, TP3, TP4, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP1"></typeparam>
        /// <typeparam name="TP2"></typeparam>
        /// <typeparam name="TP3"></typeparam>
        /// <typeparam name="TP4"></typeparam>
        /// <typeparam name="TP5"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="splitOn"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadJoin<T, TP1, TP2, TP3, TP4, TP5>(string sql, Func<T, TP1, TP2, TP3, TP4, TP5, T> func, string splitOn, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query(sql, func, param, splitOn: splitOn).SingleOrDefault();
            }
        }

        /// <summary>
        /// 加载实体及其导航属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T LoadWith<T, T1>(string sql, Func<T, List<T1>, T> func, object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1S = multiReader.Read<T1>().ToList();
                    func(entity, sub1S);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2>(string sql, Func<T, List<T1>, List<T2>, T> func, object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1S = multiReader.Read<T1>().ToList();
                    var sub2S = multiReader.Read<T2>().ToList();
                    func(entity, sub1S, sub2S);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2, T3>(string sql, Func<T, List<T1>, List<T2>, List<T3>, T> func, object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1S = multiReader.Read<T1>().ToList();
                    var sub2S = multiReader.Read<T2>().ToList();
                    var sub3S = multiReader.Read<T3>().ToList();
                    func(entity, sub1S, sub2S, sub3S);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2, T3, T4>(string sql, Func<T, List<T1>, List<T2>, List<T3>, List<T4>, T> func, object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1S = multiReader.Read<T1>().ToList();
                    var sub2S = multiReader.Read<T2>().ToList();
                    var sub3S = multiReader.Read<T3>().ToList();
                    var sub4S = multiReader.Read<T4>().ToList();
                    func(entity, sub1S, sub2S, sub3S, sub4S);
                    return entity;
                }
            }
        }

        public T LoadWith<T, T1, T2, T3, T4, T5>(string sql, Func<T, List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, T> func, object param = null)
        {
            using (var conn = GetConnection())
            {
                using (var multiReader = conn.QueryMultiple(sql, param))
                {
                    var entity = multiReader.Read<T>().SingleOrDefault();
                    var sub1S = multiReader.Read<T1>().ToList();
                    var sub2S = multiReader.Read<T2>().ToList();
                    var sub3S = multiReader.Read<T3>().ToList();
                    var sub4S = multiReader.Read<T4>().ToList();
                    var sub5S = multiReader.Read<T5>().ToList();
                    func(entity, sub1S, sub2S, sub3S, sub4S, sub5S);
                    return entity;
                }
            }
        }
        #endregion

        #region 分页列表
        public PageList<T> Query<T>(int pageIndex, int pageSize, MySearchUtil util, string cols = "*")
        {
            using (var conn = GetConnection())
            {
                return conn.PageList<T>(pageIndex, pageSize, util, cols);
            }
        }

        public PageList<T> Query<T>(string table, int pageIndex, int pageSize, MySearchUtil util, string cols = "*")
        {
            using (var conn = GetConnection())
            {
                return conn.PageList<T>(table, pageIndex, pageSize, util, cols);
            }
        }

        public PageList Query(string table, int pageIndex, int pageSize, MySearchUtil util, string cols = "*")
        {
            using (var conn = GetConnection())
            {
                return conn.PageList(table, pageIndex, pageSize, util, cols);
            }
        }
        #endregion

        #region 移除，记录操作人
        public int Remove(string table, int id, BaseAppUser user)
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(table, id, user);
            }
        }

        public int Remove(string table, int[] ids, BaseAppUser user)
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(table, ids, user);
            }
        }

        public int Remove(string table, MySearchUtil util, BaseAppUser user)
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(table, user, util);
            }
        }

        public int Remove<T>(T entity, BaseAppUser user) where T : TraceEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(entity, user);
            }
        }

        public int Remove<T>(List<T> entities, BaseAppUser user) where T : TraceEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(entities, user);
            }
        }

        public int Remove<T>(int id, BaseAppUser user) where T : TraceEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Remove<T>(id, user);
            }           
        }

        public int Remove<T>(int[] ids, BaseAppUser user) where T : TraceEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Remove<T>(ids, user);
            }
        }
        #endregion

        #region 移除，不记录操作人
        public int Remove(string table, int id)
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(table, id);
            }
        }

        public int Remove(string table, int[] ids)
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(table, ids);
            }
        }

        public int Remove(string table, MySearchUtil util)
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(table, util);
            }
        }

        public int Remove<T>(T entity) where T : BaseEntity, IRemoveAble
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(entity);
            }
        }

        public int Remove<T>(List<T> entities) where T : BaseEntity, IRemoveAble
        {
            using (var conn = GetConnection())
            {
                return conn.Remove(entities);
            }
        }

        public int Remove<T>(int id) where T : BaseEntity, IRemoveAble
        {
            using (var conn = GetConnection())
            {
                return conn.Remove<T>(id);
            }
        }

        public int Remove<T>(int[] ids) where T : BaseEntity, IRemoveAble
        {
            using (var conn = GetConnection())
            {
                return conn.Remove<T>(ids);
            }
        }
        #endregion

        #region 更新
        public int Update<T>(T entity) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Update(entity);
            }
        }

        public int UpdateInclude<T>(T entity, string[] columns) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.UpdateInclude(columns, entity);
            }
        }

        public int UpdateExclude<T>(T entity, string[] columns) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.UpdateExclude(columns, entity);
            }
        }

        public int Update<T>(List<T> entities) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Update(entities);
            }
        }

        public int UpdateInclude<T>(List<T> entities, string[] columns) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.UpdateInclude(columns, entities);
            }
        }

        public int UpdateExclude<T>(List<T> entities, string[] columns) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.UpdateExclude(columns, entities);
            }
        }

        public int Update<T>(int id, KeyValuePairs kvs) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Update<T>(id, kvs);
            }
        }

        public int Update<T>(int[] ids, KeyValuePairs kvs) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Update<T>(ids, kvs);
            }
        }

        public int Update<T>(KeyValuePairs kvs, MySearchUtil searchUtil)
        {
            using (var conn = GetConnection())
            {
                return conn.Update<T>(kvs, searchUtil);
            }
        }

        public int Update(string table, KeyValuePairs kvs, int id)
        {
            using (var conn = GetConnection())
            {
                return conn.Update(table, kvs, id);
            }
        }

        public int Update(string table, KeyValuePairs kvs, int[] ids)
        {
            using (var conn = GetConnection())
            {
                return conn.Update(table, kvs, ids);
            }
        }

        public int Update(string table, KeyValuePairs kvs, MySearchUtil searchUtil)
        {
            using (var conn = GetConnection())
            {
                return conn.Update(table, kvs, searchUtil);
            }
        }
        #endregion

        #region 数量

        public int Count<T>(MySearchUtil util = null) where T : BaseEntity
        {
            using (var conn = GetConnection())
            {
                return conn.Count<T>(util);
            }
        }

        public int Count(string table, MySearchUtil util = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Count(table, util);
            }
        }
        #endregion

        #region Dapper常用方法封装
        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// 执行sql并获取第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// 执行存储过程，返回受影响的行数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(string procName, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Execute(procName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行存储过程，返回第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteProc<T>(string procName, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(procName, param, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 以事务的方式执行一组sql语句
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExecuteTran(string[] sqls)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var s in sqls)
                        {
                            conn.Execute(s, null, tran);
                        }
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一组带参数的sql语句
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExecuteTran(KeyValuePairs sqls)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var s in sqls)
                        {
                            conn.Execute(s.Key, s.Value, tran);
                        }
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> MultiQuery<T1, T2>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(list1, list2);
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> MultiQuery<T1, T2, T3>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();
                    var list3 = multi.Read<T3>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(list1, list2, list3);
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> MultiQuery<T1, T2, T3, T4>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();
                    var list3 = multi.Read<T3>();
                    var list4 = multi.Read<T4>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(list1, list2, list3, list4);
                }
            }
        }

        /// <summary>
        /// 执行sql，返回多个列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>> MultiQuery<T1, T2, T3, T4, T5>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var conn = GetConnection())
            {
                using (var multi = conn.QueryMultiple(sql, param, commandType: commandType))
                {
                    var list1 = multi.Read<T1>();
                    var list2 = multi.Read<T2>();
                    var list3 = multi.Read<T3>();
                    var list4 = multi.Read<T4>();
                    var list5 = multi.Read<T5>();

                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>(list1, list2, list3, list4, list5);
                }
            }
        }
        #endregion
    }
}
