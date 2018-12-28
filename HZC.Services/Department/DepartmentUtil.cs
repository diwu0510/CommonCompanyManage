using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZC.Services.Department.Search;

namespace HZC.Services
{
    public class DepartmentUtil
    {
        private static List<DepartmentBll> _blls;

        private static List<DepartmentTreeDto> _tree;

        private static readonly object Lock = new object();

        #region 所有部门
        public static List<DepartmentBll> Departments()
        {
            if (_blls == null)
            {
                Init();
            }
            return _blls;
        }

        private static void Init()
        {
            var service = new DepartmentService();
            var all = service.Fetch(new DepartmentSearchParam());

            _blls = new List<DepartmentBll>();
            Dg(all, new List<int>());

            void Dg(IReadOnlyCollection<DepartmentEntity> source, IReadOnlyCollection<int> parentLevelChain, int parent = 0, int level = 1)
            {
                var root = source.Where(d => d.ParentId == parent).OrderBy(d => d.Sort);
                foreach (var r in root)
                {
                    var temp = new DepartmentBll(r)
                    {
                        LevelChain = parentLevelChain.ToList()
                    };

                    _blls.Add(temp);

                    temp.LevelChain.Add(r.Id);
                    temp.Level = level;

                    if (source.All(s => s.ParentId != r.Id)) continue;

                    level++;
                    Dg(source, temp.LevelChain, r.Id, level);
                    level--;
                }
            }
        }
        #endregion

        #region 获取部门

        public static DepartmentBll Get(int id)
        {
            return Departments().FirstOrDefault(d => d.Id == id);
        }
        #endregion

        public static List<int> GetChildrenIds(int id)
        {
            return Departments().Where(d => d.LevelChain.Contains(id)).Select(d => d.Id).ToList();
        }

        public static List<DepartmentBll> GetChildren(int id)
        {
            return Departments().Where(d => d.LevelChain.Contains(id)).ToList();
        }
        
    }
}
