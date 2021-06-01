using DMNet.Runtime.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.Runtime
{
    /// <summary>
    /// Implements all default global functions and fields
    /// </summary>
    [Bultin("/@")]
    public abstract class GlobalBase
    {
        public GlobalBase()
        {
        }

        private static GlobalBase instance;

        public static T GetInstance<T>() where T : GlobalBase
        {
            if (instance == null)
                instance = Activator.CreateInstance<T>();
            return (T)instance;
        }
    }
}
