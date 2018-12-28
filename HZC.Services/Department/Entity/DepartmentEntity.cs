using HZC.DbUtil;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HZC.Services
{
    [MyDataTable("Base_Department")]
    public class DepartmentEntity : TraceEntity
    {
        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Sort { get; set; }

        [MyDataField(Ignore = true)]
        public int MasterId { get; set; }

        // 直属下级
        public string Underling { get; set; }

        // 所有下级
        public string Children { get; set; }

        // 直属下级
        public virtual List<int> UnderlingIds => string.IsNullOrWhiteSpace(Underling)
            ? new List<int>()
            : JsonConvert.DeserializeObject<List<int>>(Underling);

        // 所有下级
        public virtual List<int> ChildrenIds => string.IsNullOrWhiteSpace(Children)
            ? new List<int>()
            : JsonConvert.DeserializeObject<List<int>>(Children);
    }
}
