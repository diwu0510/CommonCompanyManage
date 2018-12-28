using System.Linq;
using HZC.Core;
using HZC.DbUtil;
using HZC.Services.Common;

namespace HZC.Services
{
    public class DepartmentService : BaseTraceService<DepartmentEntity>
    {
        #region 设置主管

        public Result SetMaster(int departmentId, int userId)
        {
            var department = Load(departmentId);
            if (department == null || department.IsDel)
            {
                return ResultUtil.AuthFail("请求的部门不存在");
            }

            if (department.MasterId == userId)
            {
                return ResultUtil.Success();
            }

            var user = Db.LoadWith<UserEntity, DepartmentEntity>(
                "SELECT * FROM Base_User WHERE Id=@Id;SELECT * FROM Base_Department WHERE Id IN (SELECT DepartmentId FROM Base_DepartmentUser WHERE UserId=@Id)",
                (u, d) => 
                {
                    u.Departments = d;
                    return u;
                }, new { Id = userId });
            if (user == null || user.IsDel || !user.Departments.Any(d => !d.IsDel && d.Id == departmentId))
            {
                return ResultUtil.AuthFail("请求的用户不存在或该用户非指定部门的员工");
            }

            var row = Db.Update<DepartmentEntity>(
                new KeyValuePairs()
                    .Add("MasterId", userId), 
                MySearchUtil.New()
                    .AndEqual("Id", departmentId));
            return row > 0 ? ResultUtil.Success() : ResultUtil.Fail();
        }

        #endregion

        #region 验证
        public override string BeforeCreate(DepartmentEntity entity, BaseAppUser user)
        {
            return string.IsNullOrWhiteSpace(entity.Name) ? "部门名称不能为空" : string.Empty;
        }

        public override string BeforeRemove(DepartmentEntity entity, BaseAppUser user)
        {
            return string.IsNullOrWhiteSpace(entity.Name) ? "部门名称不能为空" : string.Empty;
        }

        public override string BeforeUpdate(DepartmentEntity entity, BaseAppUser user)
        {
            var count = Db.Count<DepartmentEntity>(MySearchUtil.New()
                .AndEqual("ParentId", entity.Id)
                .AndEqual("IsDel", false));
            if (count > 0)
            {
                return "此部门下存在有效子部门，禁止删除";
            }

            // 还要验证部门下是否存在有效员工
            count = Db.Count<UserEntity>(MySearchUtil.New()
                .AndEqual("IsDel", false)
                .And($"Id IN SELECT UserId FROM Base_DepartmentUser WHERE DepartmentId={entity.Id}"));
            if (count > 0)
            {
                return "此部门下存在有效员工，禁止删除";
            }
            return string.Empty;
        } 
        #endregion
    }
}
