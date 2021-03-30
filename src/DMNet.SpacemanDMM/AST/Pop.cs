using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PopConverter))]
    public class Pop
    {
        // TODO: TreePath type
        public string[] Path { get; set; }
        // TODO: Array of Tuples of String and Constant
        public Dictionary<string, Constant> Vars { get; set; } = new Dictionary<string, Constant>();
    }
}
