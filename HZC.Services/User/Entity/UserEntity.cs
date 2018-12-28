using System.Collections.Generic;
using HZC.DbUtil;

namespace HZC.Services
{
    [MyDataTable("Base_User")]
    public class UserEntity : TraceEntity
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 企业微信ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// 是否可登录
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pw { get; set; }

        /// <summary>
        /// 所在部门
        /// </summary>
        public string DepartmentIds { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string RoleIds { get; set; }

        public virtual List<DepartmentEntity> Departments { get; set; } = new List<DepartmentEntity>();
    }
}
