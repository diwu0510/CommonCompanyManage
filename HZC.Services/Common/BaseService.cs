using HZC.Core;
using HZC.DbUtil;
using System;
using System.Collections.Generic;

namespace HZC.Services
{
    public abstract class BaseService<T> where T : BaseEntity
    {
        protected MyDbUtil Db = new MyDbUtil();

        #region 添加

        public virtual Result Create(T entity, Action<bool, T> action = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var error = BeforeCreate(entity);
            if (!string.IsNullOrWhiteSpace(error)) return ResultUtil.AuthFail(error);

            entity.Id = Db.Create(entity);
            action?.Invoke(entity.Id > 0, entity);
            return entity.Id > 0 ? ResultUtil.Success(entity) : ResultUtil.Fail();
        }

        #endregion

        #region 修改

        public virtual Result Update(T entity, Action<bool, T> action = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if(entity.Id <= 0) throw new ArgumentException("实体ID无效，必须大于0", nameof(entity));

            var error = BeforeUpdate(entity);
            if (!string.IsNullOrWhiteSpace(error)) return ResultUtil.AuthFail(error);

            var row = Db.Update(entity);
            action?.Invoke(row > 0, entity);
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }
        #endregion

        #region 删除

        public virtual Result Delete(T entity, Action<bool, T> action = null)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var error = BeforeDelete(entity);
            if (!string.IsNullOrWhiteSpace(error)) return ResultUtil.AuthFail(error);

            var row = Db.Delete(entity);
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

        #region 虚方法

        public virtual string BeforeCreate(T entity)
        {
            return string.Empty;
        }

        public virtual string BeforeUpdate(T entity)
        {
            return string.Empty;
        }

        public virtual string BeforeDelete(T entity)
        {
            return string.Empty;
        }

        #endregion
    }
}
