using HZC.Core;
using HZC.DbUtil;

namespace HZC.Services
{
    public class UserSearchParam : BaseSearchParam
    {
        public string Key { get; set; }

        public int? DepartmentId { get; set; }

        public int? RoleId { get; set; }

        public int? JobId { get; set; }

        public override MySearchUtil ToSearchUtil()
        {
            var util = MySearchUtil.New().AndEqual("IsDel", false);

            if (!string.IsNullOrWhiteSpace(Key))
            {
                util.AndContains("Name", Key.Trim());
            }

            if (DepartmentId.HasValue)
            {
                util.And("Id IN (SELECT UserId FROM Base_DepartmentUser WHERE DepartmentId=@DepartmentId)",
                    new KeyValuePairs().Add("DepartmentId", DepartmentId.Value));
            }

            if (RoleId.HasValue)
            {
                util.AndEqual("Id IN (SELECT UserId FROM Base_RoleUser WHERE RoleId=@RoleId)",
                    new KeyValuePairs().Add("RoleId", RoleId.Value));
            }

            if (JobId.HasValue)
            {
                util.AndEqual("Id IN (SELECT UserId FROM Base_JobUser WHERE JobId=@JobId)",
                    new KeyValuePairs().Add("JobId", JobId.Value));
            }

            return util;
        }
    }
}
