using HZC.DbUtil;

namespace HZC.Services
{
    public class PowerEntity : TraceEntity
    {
        /// <summary>
        /// 权限组
        /// </summary>
        public string PowerGroup { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Describe { get; set; }
    }
}
