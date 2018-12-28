using HZC.DbUtil;

namespace HZC.Services
{
    [MyDataTable("Base_DepartmentUser")]
    public class DepartmentUserEntity : BaseEntity
    {
        public int DepartmentId { get; set; }

        public int UserId { get; set; }
    }
}
