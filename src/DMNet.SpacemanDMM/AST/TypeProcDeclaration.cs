using Dahomey.Cbor.Attributes;

namespace DMNet.SpacemanDMM.AST
{
    public class TypeProcDeclaration
    {
        [CborProperty("location")]
        public Location Location { get; set; }

        [CborProperty("kind")]
        public DeclKind Kind { get; set; }

        [CborProperty("id")]
        public int SymbolId { get; set; }

        [CborProperty("is_private")]
        public bool IsPrivate { get; set; }

        [CborProperty("is_protected")]
        public bool IsProtected { get; set; }

        public enum DeclKind
        {
            Proc,
            Verb,
        }
    }
}