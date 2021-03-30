using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicCodeConverter))]
    public abstract class Code
    {
        public class Present : Code
        {
            public Block Block { get; set; }
        }

        public class Invalid : Code
        {

        }

        public class Builtin : Code
        {

        }

        public class Disabled : Code
        {

        }
    }
}