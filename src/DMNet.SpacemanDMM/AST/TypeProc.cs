using Dahomey.Cbor.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    public class TypeProc
    {
        [CborProperty("name")]
        public string Name { get; set; }
        [CborProperty("values")]
        public TypeProcValue[] Values { get; set; }
        [CborProperty("declaration")]
        public TypeProcDeclaration Declaration { get; set; }
    }
}
