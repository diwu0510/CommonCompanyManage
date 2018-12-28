using System.Collections.Generic;
using System.Linq;

namespace HZC.Services
{
    public class DepartmentBll
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }

        public int ParentId { get; set; }

        public int MasterId { get; set; }

        public int Level { get; set; }

        public List<int> LevelChain { get; set; } = new List<int>();

        public DepartmentBll()
        { }

        public DepartmentBll(DepartmentEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Sort = entity.Sort;
            ParentId = entity.ParentId;
        }

        public List<DepartmentBll> GetChildren()
        {
            return DepartmentUtil.Departments().Where(d => d.LevelChain.Contains(Id)).ToList();
        }

        public DepartmentBll GetParent()
        {
            return ParentId == 0 ? null : DepartmentUtil.Departments().SingleOrDefault(d => d.Id == ParentId);
        }

        public List<DepartmentBll> GetParents()
        {
            var result = new List<DepartmentBll>();
            foreach (var lc in LevelChain)
            {
                var department = DepartmentUtil.Departments().SingleOrDefault(d => d.Id == lc);
                if (department != null)
                {
                    result.Add(department);
                }
            }

            result.Reverse();
            return result;
        }

        public UserEntity GetMaster()
        {
            if (MasterId == 0)
            {
                return null;
            }

            return null;
        }
    }
}
