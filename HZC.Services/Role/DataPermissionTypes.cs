using System.ComponentModel;

namespace HZC.Services
{
    public enum DataPermissionTypes
    {
        [Description("所有数据")]
        All = 1,
        [Description("指定数据部门")]
        Custom = 2,
        [Description("本部门及所有下属部门")]
        DeptAndChildren = 3,
        [Description("本部门")]
        Dept = 4,
        [Description("仅自己")]
        Self = 5
    }
}
