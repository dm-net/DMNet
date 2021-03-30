using Dahomey.Cbor.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    public class Spaned<T>
    {
        [CborProperty("location")]
        public Location Location { get; set; }
        [CborProperty("elem")]
        public T Value { get; set; }
    }
}
