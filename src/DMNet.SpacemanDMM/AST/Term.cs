using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicTermConverter))]
    abstract public class Term
    {
        public class Null : Term
        {
        }

        public class Int : Term
        {
            public int Value { get; set; }
        }

        public class Float : Term
        {
            public float Value { get; set; }
        }

        public class Ident : Term
        {
            public string Value { get; set; }
        }

        public class String : Term
        {
            public string Value { get; set; }
        }

        public class Resource : Term
        {
            public string Value { get; set; }
        }

        // Skiped

        public class Expr : Term
        {
            public Expression Value { get; set; }
        }

        public class Prefab : Term
        {
            public AST.Prefab Value { get; set; }
        }

        public class InterpString : Term
        {
            public string Start { get; set; }
            public Tuple<Expression, string>[] Rest { get; set; }
        }


        public class Call : Term
        {
            public string Identifier { get; set; }
            public Expression[] Args { get; set; }
        }

        public class SelfCall : Term
        {
            public Expression[] Args { get; set; }
        }

        public class ParentCall : Term
        {
            public Expression[] Args { get; set; }
        }

        public class New : Term
        {
            public NewType Type { get; set; }
            public Expression[] Args { get; set; }
        }

        public class List : Term
        {
            public Expression[] Items { get; set; }
        }

        // Skiped
    }





}
