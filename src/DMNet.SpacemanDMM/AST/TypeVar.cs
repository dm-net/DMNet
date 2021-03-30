using Dahomey.Cbor.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    public class TypeVar
    {
        [CborProperty("name")]
        public string Name { get; set; }
        [CborProperty("value")]
        public TypeVarValue Value { get; set; }
        [CborProperty("declaration")]
        public TypeVarDeclaration Declaration { get; set; }
    }
}
