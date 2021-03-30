using Dahomey.Cbor.Attributes;

namespace DMNet.SpacemanDMM.AST
{
    public class TypeVarValue
    {
        [CborProperty("location")]
        public Location Location { get; set; }

        [CborProperty("constant")]
        public Constant Constant { get; set; }

        [CborProperty("expression")]
        public Expression Expression { get; set; }
    }
}