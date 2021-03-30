using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using System;

namespace DMNet.SpacemanDMM.AST
{
    public class VarType
    {
        [CborProperty("flags")]
        [CborConverter(typeof(VarTypeFlagConverter))]
        public VarTypeFlags Flags { get; set; }

        [CborProperty("type_path")]
        public string[] Path { get; set; }

    }
    
    [Flags]
    public enum VarTypeFlags
    {
        NONE = 0,
        STATIC = 1,
        CONST = 4,
        TMP = 8,
        FINAL = 16,
        PRIVATE = 32,
        PROTECTED = 64,
    }
}