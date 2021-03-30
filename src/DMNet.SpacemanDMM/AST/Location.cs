using Dahomey.Cbor.Attributes;

namespace DMNet.SpacemanDMM.AST
{
    public class Location
    {
        [CborProperty("file")]
        public int File { get; set; }
        [CborProperty("line")]
        public int Line { get; set; }
        [CborProperty("column")]
        public int Column { get; set; }
    }
}