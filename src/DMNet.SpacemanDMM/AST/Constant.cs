using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicConstantConverter))]
    abstract public class Constant
    {
        public enum ConstFn
        {
            Icon,
            Matrix,
            Newlist,
            Sound,
            Filter,
            File,
            Generator,
        }

        public class Call : Constant
        {
            public ConstFn Function { get; set; }
            public Constant.List Args { get; set; }

            
        }

        public class Float : Constant
        {
            public float Value { get; set; }
        }

        public class Int : Constant
        {
            public int Value { get; set; }
        }

        public class List : Constant
        {
            public List<Tuple<Constant, Constant>> Items { get; set; } = new List<Tuple<Constant, Constant>>();
        }

        public class New : Constant
        {
            public Pop Type { get; set; }
            // Here it's representing a literal const of list
            public List Args { get; set; }
        }

        public class Null : Constant
        {
        }

        public class Prefab : Constant
        {
            public Pop Value { get; set; }
        }

        public class Resource : Constant
        {
            public string Value { get; set; }
        }

        public class String : Constant
        {
            public string Value { get; set; }
        }
    }
}
