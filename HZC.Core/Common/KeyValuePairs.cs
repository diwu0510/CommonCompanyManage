using System.Collections.Generic;

namespace HZC.Core
{
    public class KeyValuePairs : List<KeyValuePair<string, object>>
    {
        public KeyValuePairs Add(string key, object value)
        {
            Add(new KeyValuePair<string, object>(key, value));
            return this;
        }
    }
}
