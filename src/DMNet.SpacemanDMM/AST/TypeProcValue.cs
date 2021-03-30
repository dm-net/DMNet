using Dahomey.Cbor.Attributes;

namespace DMNet.SpacemanDMM.AST
{
    public class TypeProcValue
    {
        [CborProperty("location")]
        public Location Location { get; set; }

        [CborProperty("parameters")]
        public Parameter[] Parameters { get; set; }
        
        [CborProperty("code")]
        public Code Code { get; set; }
    }
}