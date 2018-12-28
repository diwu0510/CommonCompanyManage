using HZC.DbUtil;

namespace HZC.Services.RolePower
{
    public class RolePowerEntity : BaseEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public int PowerId { get; set; }

        /// <summary>
        /// 数据列
        /// </summary>
        public string ColumnCodeJson { get; set; }
    }
}
