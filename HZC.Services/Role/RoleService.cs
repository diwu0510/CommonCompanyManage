using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using HZC.Core;
using HZC.DbUtil;
using HZC.Services.Common;
using HZC.Services.RolePower;
using Newtonsoft.Json;

namespace HZC.Services
{
    public class RoleService
    {
        private readonly MyDbUtil _db = new MyDbUtil();

        #region 创建

        public Result Create(RoleEditDto dto, BaseAppUser user)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (user == null) throw new ArgumentNullException(nameof(user));

            var error = BeforeCrete(dto);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.Fail(error);
            }

            var entity = new RoleEntity
            {
                Name = dto.Name,
                DataPermissionType = dto.DataPermissionType,
                DepartmentIdJson = JsonConvert.SerializeObject(dto.Departments),
            };
            entity.BeforeCreate(user);

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var id = conn.Create(entity);
                        if (dto.Powers.Any())
                        {
                            conn.Create(dto.Powers
                                .Select(p => new RolePowerEntity { RoleId = id, PowerId = p })
                                .ToList());
                        }
                        trans.Commit();
                        return id > 0 ? ResultUtil.Success(id) : ResultUtil.Fail();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        conn.Close();
                        return ResultUtil.Fail(e.Message);
                    }
                }
            }
        }
        #endregion

        #region 修改
        public Result Update(RoleEditDto dto, BaseAppUser user)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (user == null) throw new ArgumentNullException(nameof(user));

            var error = BeforeUpdate(dto);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.Fail(error);
            }

            var emptyArrayJson = JsonConvert.SerializeObject(new int[] { });

            var entity = new RoleEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                DataPermissionType = dto.DataPermissionType,
                DepartmentIdJson = JsonConvert.SerializeObject(dto.Departments),
            };
            entity.BeforeCreate(user);

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var row = conn.Update(entity);
                        conn.Delete<RolePowerEntity>(MySearchUtil.New().AndEqual("RoleId", dto.Id));
                        if (dto.Powers.Any())
                        {
                            conn.Create(dto.Powers
                                .Select(p => new RolePowerEntity { RoleId = dto.Id, PowerId = p, ColumnCodeJson = emptyArrayJson})
                                .ToList());
                        }
                        trans.Commit();
                        return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        conn.Close();
                        return ResultUtil.Fail(e.Message);
                    }
                }
            }
        }
        #endregion

        #region 删除

        public Result Remove(int id, BaseAppUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var entity = _db.Load<RoleEntity>(id);
            if (entity == null)
            {
                return ResultUtil.AuthFail("请求的数据不存在");
            }

            if (entity.IsDel)
            {
                return ResultUtil.Success();
            }

            using (var conn = _db.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var row = conn.Remove(entity, user);
                        conn.Delete<RolePowerEntity>(MySearchUtil.New().AndEqual("RoleId", entity.Id));
                        trans.Commit();
                        return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        conn.Close();
                        return ResultUtil.Fail(e.Message);
                    }
                }
            }
        }
        #endregion

        #region 加载

        public RoleEditDto Load(int id)
        {
            var entity = _db.Load<RoleEntity>(id);
            if (entity == null || entity.IsDel)
            {
                return null;
            }

            var powerIds = _db.Fetch<RolePowerEntity>(MySearchUtil.New().AndEqual("RoleId", id)).Select(p => p.Id)
                .ToArray();
            return new RoleEditDto
            {
                Id = id,
                Name = entity.Name,
                DataPermissionType = entity.DataPermissionType,
                Powers = powerIds,
                Departments = JsonConvert.DeserializeObject<int[]>(entity.DepartmentIdJson)
            };
        }
        #endregion

        #region 私有方法

        public string BeforeCrete(RoleEditDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return "角色名称不能为空";
            }

            if (dto.DataPermissionType < 0 || dto.DataPermissionType > 5)
            {
                return "数据权限类型不合法";
            }

            return string.Empty;
        }

        private string BeforeUpdate(RoleEditDto dto)
        {
            return dto.Id <= 0 ? "实体Id必须大于0" : BeforeCrete(dto);
        }
        #endregion
    }
}
