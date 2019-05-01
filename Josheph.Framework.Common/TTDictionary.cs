using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{
    public class TTDictionary : Dictionary<string, MyValue>
    {
        public void Add(string key, object value1, MyEnumration.Operation value2, string Condition)
        {
            MyValue val;
            val.Value1 = value1;
            val.Value2 = value2;
            val.value3 = Condition;
            this.Add(key, val);
        }
    }
    public struct MyValue
    {
        public object Value1;
        public MyEnumration.Operation Value2;
        public string value3;
    }
}
