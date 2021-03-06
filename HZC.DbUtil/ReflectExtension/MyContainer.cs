﻿using System;
using System.Collections.Concurrent;

namespace HZC.DbUtil
{
    public class MyContainer
    {
        /// <summary>
        /// 实体及实体信息的字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, MyEntity> Dict = new ConcurrentDictionary<string, MyEntity>();

        #region 公共方法
        public static MyEntity Get(Type type)
        {
            if (!Dict.TryGetValue(type.Name, out var result))
            {
                result = MyEntityUtil.ConvertToMyEntity(type);
                Dict.TryAdd(type.Name, result);
            }

            return result;
        }
        #endregion
    }
}
