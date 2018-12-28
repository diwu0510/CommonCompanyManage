using HZC.DbUtil;

namespace HZC.Services
{
    public class JobUserEntity : BaseEntity
    {
        public int JobId { get; set; }

        public int UserId { get; set; }
    }
}
