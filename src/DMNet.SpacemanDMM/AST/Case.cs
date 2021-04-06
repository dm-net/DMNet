using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicCaseConverter))]
    public abstract class Case
    {
        public class Exact : Case
        {
            public Expression Value { get; set; }
        }

        public class Range : Case
        {
            public Expression From { get; set; }
            public Expression To { get; set; }
        }
    }
}
