using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Verification
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    class RequiredAttribute : Attribute
    {
    }
}
