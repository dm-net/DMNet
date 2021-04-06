using Dahomey.Cbor.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    public class VarStatement
    {
        [CborProperty("var_type")]
        public VarType VarType { get; set; }

        [CborProperty("name")]
        public string Name { get; set; }

        [CborProperty("value")]
        public Expression Value { get; set; }
    }
}
