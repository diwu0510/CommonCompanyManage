using System;
using HZC.Core;
using Newtonsoft.Json;

namespace HZC.DbUtil
{
    public class TraceEntity : BaseEntity, IRemoveAble
    {
        [MyDataField(UpdateIgnore = true)]
        public bool IsDel { get; set; } = false;

        [JsonConverter(typeof(DateTimeFormatConverter))]
        [MyDataField(UpdateIgnore = true)]
        public DateTime CreateAt { get; set; } = DateTime.Now;

        [MyDataField(UpdateIgnore = true)]
        public string Creator { get; set; }

        [JsonConverter(typeof(DateTimeFormatConverter))]
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        public string Updator { get; set; }

        public void BeforeCreate(BaseAppUser user)
        {
            Creator = user.Name;
            Updator = user.Name;
        }

        public void BeforeUpdate(BaseAppUser user)
        {
            Updator = user.Name;
            UpdateAt = DateTime.Now;
        }
    }
}
