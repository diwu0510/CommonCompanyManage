using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HZC.Core;

namespace HZC.DbUtil
{
    public class MySearchUtil
    {
        // 查询参数
        private readonly DynamicParameters _parameters = new DynamicParameters();
        // 排序子句集合
        private readonly List<string> _orderBy = new List<string>();

        private readonly string _prefix;
        // where
        private string _where = "1=1";
        // 参数序号
        private int _idx;

        public MyClauses SubClauses()
        {
            return new MyClauses();
        }

        #region 构造方法
        public MySearchUtil(string prefix = "@")
        {
            _prefix = string.IsNullOrWhiteSpace(prefix) ? "@" : prefix;
        }

        public static MySearchUtil New(string prefix = "@")
        {
            return new MySearchUtil(prefix);
        }
        #endregion

        #region 主语句
        public MySearchUtil And(string clause)
        {
            _where += $" AND {clause}";
            return this;
        }

        /// <summary>
        /// 注意不要使用_p[数字]形式的参数名和重复的参数名
        /// </summary>
        /// <param name="clause"></param>
        /// <param name="kvs"></param>
        /// <returns></returns>
        public MySearchUtil And(string clause, KeyValuePairs kvs)
        {
            _where += $" AND {clause}";
            foreach (var kv in kvs)
            {
                _parameters.Add(kv.Key, kv.Value);
            }
            return this;
        }

        public MySearchUtil Or(string clause)
        {
            _where = $"({_where}) OR ({clause})";
            return this;
        }

        public MySearchUtil AndOr(MyClauses scs)
        {
            var clauses = new List<string>();
            foreach (var sc in scs)
            {
                clauses.Add(ConvertSearchClause(sc));
            }

            _where = $"{_where} AND ({string.Join(" OR ", clauses)})";
            return this;
        }
        #endregion

        #region 获取查询语句和参数
        public string GetWhere()
        {
            return _where;
        }

        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <param name="isPagerParameter">是否返回分页查询参数</param>
        /// <returns></returns>
        public DynamicParameters GetParam(bool isPagerParameter = false)
        {
            if (isPagerParameter)
            {
                _parameters.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            }

            return _parameters;
        }

        public string GetOrderBy() => _orderBy.Any() ? string.Join(",", _orderBy) : "Id";
        #endregion

        #region And
        public MySearchUtil AndEqual(string column, object value)
        {
            var clause = $"{column}={AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndNotEqual(string column, object value)
        {
            var clause = $"{column}<>{AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndGreaterThan(string column, object value)
        {
            var clause = $"{column}>{AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndGreaterThanEqual(string column, object value)
        {
            var clause = $"{column}>={AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndLessThan(string column, object value)
        {
            var clause = $"{column}<{AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndLessThanEqual(string column, object value)
        {
            var clause = $"{column}<={AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndContains(string column, string value)
        {
            var clause = $"{column} LIKE {AddParameter("%" + value + "%")}";
            return And(clause);
        }

        public MySearchUtil AndContains(string[] columns, string value)
        {
            if (!columns.Any()) return this;

            var pName = AddParameter("%" + value + "%");
            var clauses = columns.Select(c => $"{c} LIKE {pName}");
            return And($"({string.Join(" OR ", clauses)})");
        }

        public MySearchUtil AndStartWith(string column, string value)
        {
            var clause = $"{column} LIKE {AddParameter(value + "%")}";
            return And(clause);
        }

        public MySearchUtil AndStartWith(string[] columns, string value)
        {
            if (!columns.Any()) return this;

            var pName = AddParameter(value + "%");
            var clauses = columns.Select(c => $"{c} LIKE {pName}");
            return And($"({string.Join(" OR ", clauses)})");
        }

        public MySearchUtil AndEndWith(string column, string value)
        {
            var clause = $"{column} LIKE {AddParameter("%" + value)}";
            return And(clause);
        }

        public MySearchUtil AndEndWith(string[] columns, string value)
        {
            if (!columns.Any()) return this;

            var pName = AddParameter("%" + value);
            var clauses = columns.Select(c => $"{c} LIKE {pName}");
            return And($"({string.Join(" OR ", clauses)})");
        }

        public MySearchUtil AndIn(string column, int[] value)
        {
            var clause = $"{column} IN {AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndIn(string column, string[] value)
        {
            var clause = $"{column} IN {AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndIn(string column, DateTime[] value)
        {
            var clause = $"{column} IN {AddParameter(value)}";
            return And(clause);
        }

        public MySearchUtil AndIsNull(string column)
        {
            var clause = $"{column} IS NULL";
            return And(clause);
        }

        public MySearchUtil AndIsNotNull(string column)
        {
            var clause = $"{column} IS NOT NULL";
            return And(clause);
        }
        #endregion

        #region Or
        public MySearchUtil OrEqual(string column, object value)
        {
            var clause = $"{column}={AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrNotEqual(string column, object value)
        {
            var clause = $"{column}<>{AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrGreaterThan(string column, object value)
        {
            var clause = $"{column}>{AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrGreaterThanEqual(string column, object value)
        {
            var clause = $"{column}>={AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrLessThan(string column, object value)
        {
            var clause = $"{column}<{AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrLessThanEqual(string column, object value)
        {
            var clause = $"{column}<={AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrContains(string column, string value)
        {
            var clause = $"{column} LIKE {AddParameter("%" + value + "%")}";
            return Or(clause);
        }

        public MySearchUtil OrContains(string[] columns, string value)
        {
            if (!columns.Any()) return this;

            var pName = AddParameter("%" + value + "%");
            var clauses = columns.Select(c => $"{c} LIKE {pName}");
            return Or($"({string.Join(" OR ", clauses)})");
        }

        public MySearchUtil OrStartWith(string column, string value)
        {
            var clause = $"{column} LIKE {AddParameter(value + "%")}";
            return Or(clause);
        }

        public MySearchUtil OrStartWith(string[] columns, string value)
        {
            if (!columns.Any()) return this;

            var pName = AddParameter(value + "%");
            var clauses = columns.Select(c => $"{c} LIKE {pName}");
            return Or($"({string.Join(" OR ", clauses)})");
        }

        public MySearchUtil OrEndWith(string column, string value)
        {
            var clause = $"{column} LIKE {AddParameter("%" + value)}";
            return Or(clause);
        }

        public MySearchUtil OrEndWith(string[] columns, string value)
        {
            if (!columns.Any()) return this;

            var pName = AddParameter("%" + value);
            var clauses = columns.Select(c => $"{c} LIKE {pName}");
            return Or($"({string.Join(" OR ", clauses)})");
        }

        public MySearchUtil OrIn(string column, int[] value)
        {
            var clause = $"{column} IN {AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrIn(string column, string[] value)
        {
            var clause = $"{column} IN {AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrIn(string column, DateTime[] value)
        {
            var clause = $"{column} IN {AddParameter(value)}";
            return Or(clause);
        }

        public MySearchUtil OrIsNull(string column)
        {
            var clause = $"{column} IS NULL";
            return Or(clause);
        }

        public MySearchUtil OrIsNotNull(string column)
        {
            var clause = $"{column} IS NOT NULL";
            return Or(clause);
        }
        #endregion

        #region 私有方法
        // 添加查询参数
        private string AddParameter(object val)
        {
            var pName = $"{_prefix}_p{_idx++}";
            _parameters.Add(pName, val);

            return pName;
        }

        // 处理查询子句
        private string ConvertSearchClause(MyClause sc)
        {
            string pName;
            string op;

            switch (sc.Op)
            {
                case MyOps.Equal:
                    pName = AddParameter(sc.Value);
                    op = "=";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.NotEqual:
                    pName = AddParameter(sc.Value);
                    op = "<>";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.GreaterThan:
                    pName = AddParameter(sc.Value);
                    op = ">";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.GreaterThanEqual:
                    pName = AddParameter(sc.Value);
                    op = ">=";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.LessThan:
                    pName = AddParameter(sc.Value);
                    op = "<";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.LessThanEqual:
                    pName = AddParameter(sc.Value);
                    op = "<=";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.Contains:
                    pName = AddParameter("%" + sc.Value + "%");
                    op = "LIKE";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.StartWith:
                    pName = AddParameter(sc.Value + "%");
                    op = "LIKE";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.EndWith:
                    pName = AddParameter("%" + sc.Value);
                    op = "LIKE";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.In:
                    pName = AddParameter(sc.Value);
                    op = "IN";
                    return $"{sc.Key} {op} {pName}";
                case MyOps.NotNull:
                    return $"{sc.Key} NOT NULL";
                case MyOps.IsNull:
                    return $"{sc.Key} IS NULL";
            }

            return "";
        }

        // 排序
        public MySearchUtil OrderBy(string key)
        {
            _orderBy.Add(key);
            return this;
        }

        public MySearchUtil OrderByDesc(string key)
        {
            _orderBy.Add($"{key} DESC");
            return this;
        }
        #endregion
    }

    public enum MyOps
    {
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        GreaterThanEqual = 4,
        LessThan = 5,
        LessThanEqual = 6,
        Contains = 7,
        StartWith = 8,
        EndWith = 9,
        In = 10,
        NotNull = 11,
        IsNull = 12
    }

    public class MyClause
    {
        public string Key { get; set; }

        public MyOps Op { get; set; }

        public object Value { get; set; }

        public MyClause(string key, MyOps op, object value)
        {
            Key = key;
            Op = op;
            Value = value;
        }
    }

    public class MyClauses : List<MyClause>
    {
        public static MyClauses New()
        {
            return new MyClauses();
        }

        public MyClauses Add(string key, MyOps op, object value)
        {
            Add(new MyClause(key, op, value));
            return this;
        }
    }
}
