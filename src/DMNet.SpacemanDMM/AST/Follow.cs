using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicFollowConverter))]
    public abstract class Follow
    {
        public class Index : Follow
        {
            public Expression Expression { get; set; }
        }

        public class Field : Follow
        {
            new public IndexKind Index { get; set; }
            public string Identifier { get; set; }
        }

        public class Call : Follow
        {
            new public IndexKind Index { get; set; }
            public string Identifier { get; set; }
            public Expression[] Arguments { get; set; }
        }

        public enum IndexKind
        {
            /// `a.b`
            Dot,
            /// `a:b`
            Colon,
            /// `a?.b`
            SafeDot,
            /// `a?:b`
            SafeColon,
        }

    }
}
