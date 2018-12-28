using HZC.DbUtil;

namespace HZC.Services.Department.Search
{
    public class DepartmentSearchParam : BaseSearchParam
    {
        public string Key { get; set; }

        public int? ParentId { get; set; }

        public override MySearchUtil ToSearchUtil()
        {
            var util = MySearchUtil.New().AndEqual("IsDel", false);

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains("Name", Key.Trim());
            }

            if (ParentId.HasValue)
            {
                util.AndEqual("ParentId", ParentId.Value);
            }

            return util;
        }
    }
}
