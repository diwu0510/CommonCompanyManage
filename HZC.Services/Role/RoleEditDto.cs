namespace HZC.Services
{
    public class RoleEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        public int DataPermissionType { get; set; }

        public int[] Powers { get; set; } = { };

        public int[] Departments { get; set; } = {};
    }
}
