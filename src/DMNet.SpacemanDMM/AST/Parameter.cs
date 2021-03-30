using Dahomey.Cbor.Attributes;

namespace DMNet.SpacemanDMM.AST
{
    public class Parameter
    {
        [CborProperty("var_type")]
        public VarType VarType { get; set; }

        [CborProperty("name")]
        public string Name { get; set; }

        [CborProperty("default")]
        public Expression Default { get; set; }

        /* TODO: ImputType
        [CborProperty("input_type")]
        public Expression Default { get; set; }
        */

        [CborProperty("in_list")]
        public Expression InList { get; set; }

        [CborProperty("location")]
        public Location Location { get; set; }
    }
}