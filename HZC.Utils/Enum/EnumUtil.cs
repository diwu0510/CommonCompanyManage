using System;
using System.Collections.Generic;
using System.Text;

namespace HZC.Utils
{
    public class EnumUtil
    {
        public T ConvertToEnum<T>(int num)
        {
            if (Enum.IsDefined(typeof(T), num))
            {
                return (T) Enum.ToObject(typeof(T), num);
            }

            return default(T);
        }
    }
}
