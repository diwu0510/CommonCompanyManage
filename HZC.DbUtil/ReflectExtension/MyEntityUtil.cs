using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HZC.DbUtil
{
    public static class MyEntityUtil
    {
        #region 构造INSERT和UPDATE的SQL语句
        /// <summary>
        /// 构造插入语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string BuildCommonInsertSqlStatement(MyEntity entity)
        {
            var sb = new StringBuilder();
            var cols = new List<string>();
            var props = new List<string>();

            foreach (var p in entity.Properties)
            {
                if (!p.Ignore && !p.InsertIgnore && !p.IsPrimary)
                {
                    cols.Add(p.DataBaseColumn);
                    props.Add($"@{p.PropertyName}");
                }
            }

            sb.Append($"INSERT INTO [{entity.Table}] (");
            sb.Append(string.Join(',', cols));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', props));
            sb.Append(");SELECT @@IDENTITY;");

            return sb.ToString();
        }

        /// <summary>
        /// 构造更新语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string BuildCommonUpdateSqlStatement(MyEntity entity)
        {
            var sb = new StringBuilder();
            var cols = new List<string>();

            foreach (var p in entity.Properties)
            {
                if (!p.Ignore && !p.UpdateIgnore && !p.IsPrimary)
                {
                    cols.Add($"{p.DataBaseColumn}=@{p.PropertyName}");
                }
            }

            sb.Append($"UPDATE [{entity.Table}] SET ");
            sb.Append(string.Join(',', cols));
            sb.Append(" WHERE Id=@Id");

            if (entity.HasVersion)
            {
                sb.Append(" AND Version=@Version");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 构造插入语句
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="columns">要包含（或忽略）的列</param>
        /// <param name="isExclude">指定列的包含方式。默认为false，既只插入columns指定的列。若设置为true，则插入除columns指定列外的其他列</param>
        /// <returns></returns>
        public static string BuildInsertSqlStatement(MyEntity entity, string[] columns, bool isExclude = false)
        {
            var includeColumns = isExclude
                ? entity.Properties.Where(p => !columns.Contains(p.PropertyName) && !p.Ignore && !p.InsertIgnore).ToList()
                : entity.Properties.Where(p => columns.Contains(p.PropertyName) && !p.Ignore && !p.InsertIgnore).ToList();

            if (includeColumns.Count == 0)
            {
                throw new ArgumentNullException(nameof(columns), "指定字段中未包含有效列");
            }

            var sb = new StringBuilder();
            var cols = new List<string>();
            var props = new List<string>();

            foreach (var p in includeColumns)
            {
                cols.Add(p.DataBaseColumn);
                props.Add($"@{p.PropertyName}");
            }

            sb.Append($"INSERT INTO [{entity.Table}] (");
            sb.Append(string.Join(',', cols));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', props));
            sb.Append(");SELECT @@IDENTITY;");

            return sb.ToString();
        }

        public static string BuildInsertInclude(MyEntity entity, string[] include)
        {
            var includeColumns = entity.Properties.Where(p => include.Contains(p.PropertyName) && !p.Ignore && !p.InsertIgnore).ToList();

            if (includeColumns.Count == 0)
            {
                throw new ArgumentNullException(nameof(include), "指定字段中未包含有效列");
            }

            var sb = new StringBuilder();
            var cols = new List<string>();
            var props = new List<string>();

            foreach (var p in includeColumns)
            {
                cols.Add(p.DataBaseColumn);
                props.Add($"@{p.PropertyName}");
            }

            sb.Append($"INSERT INTO [{entity.Table}] (");
            sb.Append(string.Join(',', cols));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', props));
            sb.Append(");SELECT @@IDENTITY;");

            return sb.ToString();
        }

        public static string BuildInsertExclude(MyEntity entity, string[] exclude)
        {
            var includeColumns = entity.Properties.Where(p => !exclude.Contains(p.PropertyName) && !p.Ignore && !p.InsertIgnore).ToList();

            if (includeColumns.Count == 0)
            {
                throw new ArgumentNullException(nameof(exclude), "指定字段中未包含有效列");
            }

            var sb = new StringBuilder();
            var cols = new List<string>();
            var props = new List<string>();

            foreach (var p in includeColumns)
            {
                cols.Add(p.DataBaseColumn);
                props.Add($"@{p.PropertyName}");
            }

            sb.Append($"INSERT INTO [{entity.Table}] (");
            sb.Append(string.Join(',', cols));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', props));
            sb.Append(");SELECT @@IDENTITY;");

            return sb.ToString();
        }

        /// <summary>
        /// 构造查询语句
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="columns">要包含（或忽略的列）</param>
        /// <param name="isExclude">指定列的包含方式。默认为false，既只更新columns指定的列。若设置为true，则更新除columns指定列外的其他列</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string BuildUpdateSqlStatement(MyEntity entity, string[] columns, bool isExclude = false)
        {
            var includeColumns = isExclude
                ? entity.Properties.Where(p => !columns.Contains(p.PropertyName) && !p.Ignore && !p.UpdateIgnore).ToList()
                : entity.Properties.Where(p => columns.Contains(p.PropertyName) && !p.Ignore && !p.UpdateIgnore).ToList();

            if (!includeColumns.Any())
            {
                throw new ArgumentNullException(nameof(columns), "指定字段中未包含有效列");
            }

            var sb = new StringBuilder();
            var cols = new List<string>();

            foreach (var p in includeColumns)
            {
                cols.Add($" {p.DataBaseColumn}=@{p.PropertyName}");
            }

            sb.Append($"UPDATE [{entity.Table}] SET ");
            sb.Append(string.Join(',', cols));
            sb.Append(" WHERE Id=@Id");

            return sb.ToString();
        }

        /// <summary>
        /// 从匿名类型中获取插入的sql语句
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="obj">匿名对象</param>
        /// <returns></returns>
        public static string BuildInsertSqlByAnonymous(MyEntity entity, object obj)
        {
            var objProps = obj.GetType().GetProperties().Select(p => p.Name);
            var includeColumns = entity.Properties.Where(p => objProps.Contains(p.PropertyName) && !p.InsertIgnore && !p.Ignore).ToList();

            if (!includeColumns.Any())
            {
                throw new ArgumentNullException(nameof(obj), "指定字段中未包含有效列");
            }

            var sb = new StringBuilder();
            var cols = new List<string>();
            var props = new List<string>();

            foreach (var p in includeColumns)
            {
                cols.Add(p.DataBaseColumn);
                props.Add($"@{p.PropertyName}");
            }

            sb.Append($"INSERT INTO [{entity.Table}] (");
            sb.Append(string.Join(',', cols));
            sb.Append(") VALUES (");
            sb.Append(string.Join(',', props));
            sb.Append(");SELECT SCOPE_IDENTITY();");

            return sb.ToString();
        }

        /// <summary>
        /// 从匿名类型中获取更新的Sql语句
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="obj">匿名对象</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static string BuildUpdateSqlByAnonymous(MyEntity entity, object obj, string where = "")
        {
            var props = obj.GetType().GetProperties().Select(p => p.Name);
            var includeColumns = entity.Properties.Where(p => props.Contains(p.PropertyName)).ToList();

            if (!includeColumns.Any())
            {
                throw new ArgumentNullException(nameof(obj), "指定字段中未包含有效列");
            }

            where = string.IsNullOrWhiteSpace(where) ? "1=1" : where;

            if (includeColumns.Any(c => c.DataBaseColumn == "Id"))
            {
                where += " Id=@Id";
            }

            var sb = new StringBuilder();
            var cols = new List<string>();

            foreach (var p in includeColumns)
            {
                cols.Add($"{p.DataBaseColumn}=@{p.PropertyName}");
            }

            sb.Append($"UPDATE [{entity.Table}] SET ");
            sb.Append(string.Join(',', cols));
            sb.Append($" WHERE {where}");

            return sb.ToString();
        }
        #endregion

        #region PropertyInfo转换为MyPropertyAttribute

        /// <summary>
        /// 将PropertyInfo转换为MyPropertyAttribute
        /// </summary>
        /// <returns></returns>
        internal static MyEntity ConvertToMyEntity(Type type)
        {
            var entity = new MyEntity { Name = type.Name };

            var entityAttribute = type.GetCustomAttribute<MyDataTableAttribute>(false);

            entity.Table = entityAttribute != null ? entityAttribute.Name : type.Name.Replace("Entity", "");

            entity.Properties = new List<MyProperty>();

            var properties = type.GetProperties();
            foreach (var p in properties)
            {
                if (!CheckPropertyType(p))
                {
                    // 不是可支持的数据类型，直接返回
                    continue;
                }

                if (p.Name == "Id")
                {
                    entity.Properties.Add(new MyProperty
                    {
                        DataBaseColumn = "Id",
                        IsPrimary = true,
                        InsertIgnore = true,
                        UpdateIgnore = true
                    });
                    continue;
                }

                if (p.Name == "Version")
                {
                    entity.Properties.Add(new MyProperty
                    {
                        DataBaseColumn = "Version",
                        InsertIgnore = true,
                        UpdateIgnore = true
                    });
                    entity.HasVersion = true;
                }

                var attribute = p.GetCustomAttribute<MyDataFieldAttribute>(false);
                if (attribute == null)
                {
                    entity.Properties.Add(new MyProperty
                    {
                        DataBaseColumn = p.Name,
                        PropertyName = p.Name
                    });
                }
                else
                {
                    entity.Properties.Add(new MyProperty
                    {
                        PropertyName = p.Name,
                        DataBaseColumn = string.IsNullOrWhiteSpace(attribute.ColumnName) ? p.Name : attribute.ColumnName,
                        InsertIgnore = attribute.InsertIgnore,
                        UpdateIgnore = attribute.UpdateIgnore,
                        Ignore = attribute.Ignore
                    });
                }
            }

            entity.InsertSqlStatement = BuildCommonInsertSqlStatement(entity);
            entity.UpdateSqlStatement = BuildCommonUpdateSqlStatement(entity);

            return entity;
        }

        /// <summary>
        /// 检查指定属性是否是受支持的数据类型
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool CheckPropertyType(PropertyInfo property)
        {
            var types = new[]
            {
                "Byte", "SByte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64", "Single", "Double", "Boolean",
                "String", "Char", "Guid", "DateTime", "Byte[]", "DateTimeOffset"
            };

            var realType = GetPropertyRealType(property);
            return types.Contains(realType);
        }

        private static string GetPropertyRealType(PropertyInfo property)
        {
            var typeName = property.PropertyType.IsGenericType &&
                              property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? property.PropertyType.GetGenericArguments()[0].Name
                : property.PropertyType.Name;

            return typeName;
        }
        #endregion
    }
}
