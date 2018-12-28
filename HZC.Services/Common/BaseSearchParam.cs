using System;
using System.Collections.Generic;
using System.Text;
using HZC.DbUtil;

namespace HZC.Services
{
    public abstract class BaseSearchParam
    {
        public abstract MySearchUtil ToSearchUtil();
    }
}
