﻿using System;

namespace HZC.DbUtil
{
    /// <summary>
    /// 数据实体-数据列配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MyDataFieldAttribute : Attribute
    {
        /// <summary>
        /// 属性列对应的数据库字段名
        /// </summary>
        public string ColumnName { get; set; } = "";

        /// <summary>
        /// Insert操作时忽略此属性
        /// </summary>
        public bool InsertIgnore { get; set; } = false;

        /// <summary>
        /// Update操作时忽略此属性
        /// </summary>
        public bool UpdateIgnore { get; set; } = false;

        /// <summary>
        /// Inser和Update时忽略此属性
        /// </summary>
        public bool Ignore { get; set; } = false;
    }
}
