using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;

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

        [CborProperty("input_type")]
        [CborConverter(typeof(FlagConverter<InputType>))]
        public InputType InputType { get; set; }
        

        [CborProperty("in_list")]
        public Expression InList { get; set; }

        [CborProperty("location")]
        public Location Location { get; set; }
    }
}