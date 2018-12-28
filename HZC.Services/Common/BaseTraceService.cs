using HZC.Core;
using HZC.DbUtil;
using System;
using System.Collections.Generic;

namespace HZC.Services.Common
{
    public class BaseTraceService<T> where T : TraceEntity
    {
        protected MyDbUtil Db = new MyDbUtil();

        #region 创建
        public virtual Result Create(T entity, BaseAppUser user, Action<bool, T> action = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (user == null) throw new ArgumentNullException(nameof(user), "可跟踪实体必须提供操作人信息");

            entity.BeforeCreate(user);

            var error = BeforeCreate(entity, user);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.AuthFail(error);
            }

            entity.Id = Db.Create(entity);
            var success = entity.Id > 0;
            action?.Invoke(success, entity);
            return success ? ResultUtil.Success(entity) : ResultUtil.Fail();
        }
        #endregion

        #region 修改
        public virtual Result Update(T entity, BaseAppUser user, Action<bool, T> action = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Id <= 0) throw new ArgumentException("实体ID无效，必须大于0", nameof(entity));
            if (user == null) throw new ArgumentNullException(nameof(user), "可跟踪实体必须提供操作人信息");

            entity.BeforeUpdate(user);

            var error = BeforeUpdate(entity, user);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.AuthFail(error);
            }

            var row = Db.Update(entity);
            var success = row > 0;
            action?.Invoke(success, entity);
            return success ? ResultUtil.Success() : ResultUtil.Fail();
        }
        #endregion

        #region 移除
        public virtual Result Remove(T entity, BaseAppUser user, Action<bool, T> action = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (user == null) throw new ArgumentNullException(nameof(user), "可跟踪实体必须提供操作人信息");

            var error = BeforeRemove(entity, user);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.AuthFail(error);
            }

            var row = Db.Remove(entity, user);
            action?.Invoke(row > 0, entity);
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }
        #endregion

        #region 列表

        public List<T> Fetch(BaseSearchParam param = null)
        {
            var util = param?.ToSearchUtil() ?? new MySearchUtil();
            return Db.Fetch<T>(util);
        }
        #endregion

        #region 分页列表

        public PageList<T> Query(int pageIndex, int pageSize, BaseSearchParam param = null)
        {
            var util = param?.ToSearchUtil() ?? new MySearchUtil();
            return Db.Query<T>(pageIndex, pageSize, util);
        }

        #endregion

        #region 加载实体

        public T Load(int id)
        {
            return Db.Load<T>(id);
        }

        #endregion

        #region 验证

        public virtual string BeforeCreate(T entity, BaseAppUser user)
        {
            return string.Empty;
        }

        public virtual string BeforeUpdate(T entity, BaseAppUser user)
        {
            return string.Empty;
        }

        public virtual string BeforeRemove(T entity, BaseAppUser user)
        {
            return string.Empty;
        }
        #endregion
    }
}
