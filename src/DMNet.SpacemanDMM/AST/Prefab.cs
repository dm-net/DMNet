using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PrefabConverter))]
    public class Prefab
    {
        public Tuple<Operators.PathOp, string>[] Path { get; set; }

        public Dictionary<string, Expression> Vars { get; set; } = new Dictionary<string, Expression>();
    }
}
