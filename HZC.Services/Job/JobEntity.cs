using HZC.DbUtil;

namespace HZC.Services
{
    public class JobEntity : TraceEntity
    {
        /// <summary>
        /// 职位组
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string Name { get; set; }
    }
}
