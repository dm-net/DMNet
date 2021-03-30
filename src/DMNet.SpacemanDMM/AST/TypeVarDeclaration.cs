using Dahomey.Cbor.Attributes;

namespace DMNet.SpacemanDMM.AST
{
    public class TypeVarDeclaration
    {
        [CborProperty("location")]
        public Location Location { get; set; }

        [CborProperty("var_type")]
        public VarType Type { get; set; }

        [CborProperty("id")]
        public int SymbolId { get; set; }
    }
}