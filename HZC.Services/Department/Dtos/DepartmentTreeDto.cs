using System;
using System.Collections.Generic;
using System.Text;

namespace HZC.Services
{
    public class DepartmentTreeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Level { get; set; }

        public bool IsLeaf { get; set; }

        public List<DepartmentTreeDto> Children { get; set; } = new List<DepartmentTreeDto>();

        public DepartmentTreeDto()
        { }

        public DepartmentTreeDto(DepartmentEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ParentId = entity.ParentId;
        }
    }
}
