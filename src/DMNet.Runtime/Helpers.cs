using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.Runtime
{
    public static class Helpers
    {


        public static bool IsTrue(object obj)
        {
            if (obj is float f)
                return f != 0f;
            if (obj is bool b)
                return b;
            if (obj is int i)
                return i != 0;
            if (obj is string s)
                return !string.IsNullOrEmpty(s);

            return obj != null;
        }
    }
}
