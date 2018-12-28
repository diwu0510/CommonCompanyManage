using HZC.DbUtil;

namespace HZC.Services
{
    public class RoleUserEntity : BaseEntity
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }
    }
}
