using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.Runtime.Attributes
{
    /// <summary>
    /// Tells DMNet compiler that this is a bltin with specified name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class BultinAttribute : Attribute
    {
        public BultinAttribute()
        {
        }

        public BultinAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

    }
}
