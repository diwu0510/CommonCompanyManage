using HZC.DbUtil;

namespace HZC.Services
{
    public class MenuEntity : TraceEntity
    {
        /// <summary>
        /// 上级菜单
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public string PowerCode { get; set; }

        /// <summary>
        /// 菜单文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; } = true;
    }
}
