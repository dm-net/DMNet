using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicExpressionConverter))]
    abstract public class Expression
    {
        public class Base : Expression
        {
            /// <summary>
            /// The unary operations applied to this value, in reverse order.
            /// </summary>
            public Operators.UnaryOp[] UnaryOps { get; set; }

            public Spaned<Term> Term { get; set; }

            public Spaned<Follow>[] Follows { get; set; }
        }

        public class BinaryOp : Expression
        {
            public Operators.BinaryOp Op { get; set; }
            public Expression Lhs { get; set; }
            public Expression Rhs { get; set; }
        }

        public class AssignOp : Expression
        {
            public Operators.AssignOp Op { get; set; }
            public Expression Lhs { get; set; }
            public Expression Rhs { get; set; }
        }

        public class TernaryOp : Expression
        {
            public Expression Condition { get; set; }
            public Expression True { get; set; }
            public Expression False { get; set; }
        }
    }

    
}
