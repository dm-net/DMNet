using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using DMNet.SpacemanDMM.AST.Verification;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicNewTypeConverter))]
    public abstract class NewType
    {
        public class Implicit : NewType
        {

        }

        public class Prefab : NewType
        {
            public AST.Prefab Value { get; set; }
        }

        public class MiniExpr : NewType
        {
            [Required]
            public string Identifier { get; set; }
            public Follow[] Fields { get; set; }
        }
    }
}
