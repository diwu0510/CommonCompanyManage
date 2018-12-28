using HZC.DbUtil;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HZC.Services
{
    public class RoleEntity : TraceEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据权限类型
        /// </summary>
        public int DataPermissionType { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentIdJson { get; set; } = JsonConvert.SerializeObject(new List<int>());

        public List<PowerEntity> PowerIds { get; set; }

        //public List<int> DepartmentIds => string.IsNullOrWhiteSpace(DepartmentIdJson)
        //    ? new List<int>()
        //    : JsonConvert.DeserializeObject<List<int>>(DepartmentIdJson);
    }
}
