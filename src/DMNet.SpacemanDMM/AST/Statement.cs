using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;
using DMNet.SpacemanDMM.AST.Verification;
using System;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicStatementConverter))]
    public class Statement
    {
        public class Expr : Statement
        {
            [Required]
            public Expression Expression { get; set; }
        }

        public class Return : Statement
        {
            public Expression Expression { get; set; }
        }

        public class Throw : Statement
        {
            public Expression Expression { get; set; }
        }

        public class While : Statement
        {
            [Required]
            public Expression Condition { get; set; }
            [Required]
            public Block Block { get; set; }
        }

        public class DoWhile : Statement
        {
            [Required]
            public Block Block { get; set; }

            [Required]
            public Spaned<Expression> Condition { get; set; }
        }

        public class If : Statement
        {
            [Required]
            public Tuple<Spaned<Expression>, Block>[] Arms { get; set; }

            public Block Else { get; set; }
        }

        public class ForLoop : Statement
        {
            public Statement Init { get; set; }
            public Expression Test { get; set; }
            public Statement Inc { get; set; }
            [Required]
            public Block Block { get; set; }
        }
        public class ForList : Statement
        {
            public VarType VarType { get; set; }
            [Required]
            public string Identifier { get; set; }
            public InputType InputType { get; set; }
            public Expression List { get; set; }
            [Required]
            public Block Block { get; set; }
        }

        public class ForRange : Statement
        {
            public VarType VarType { get; set; }
            [Required]
            public string Identifier { get; set; }
            [Required]
            public Expression Start { get; set; }
            [Required]
            public Expression End { get; set; }
            public Expression Step { get; set; }
            [Required]
            public Block Block { get; set; }
        }

        public class Var : Statement
        {
            public VarStatement Value { get; set; }
        }

        public class Vars : Statement
        {
            public VarStatement[] Value { get; set; }
        }

        public class Setting : Statement
        {
            [Required]
            public string Name { get; set; }
            public SettingMode Mode { get; set; }
            [Required]
            public Expression Value { get; set; }
        }
        public class Spawn : Statement
        {
            public Expression Delay { get; set; }
            [Required]
            public Block Block { get; set; }
        }

        public class Switch : Statement
        {
            [Required]
            public Expression Input { get; set; }

            public Tuple<Spaned<Case[]>, Block>[] Cases { get; set; }

            public Block Default { get; set; }
        }

        public class TryCatch : Statement
        {
            public Block Try { get; set; }
            public string[][] Params { get; set; }
            public Block Catch { get; set; }
        }

        public class Continue : Statement
        {
            public string Identifier { get; set; }
        }

        public class Break : Statement
        {
            public string Identifier { get; set; }
        }

        public class Goto : Statement
        {
            public string Identifier { get; set; }
        }

        public class Label : Statement
        {
            public string Identifier { get; set; }
            public Block Block { get; set; }
        }

        public class Del : Statement
        {
            [Required]
            public Expression Expression { get; set; }
        }
        public class Crash : Statement
        {
            [Required]
            public Expression Expression { get; set; }
        }

        
    }

    public enum SettingMode
    {
        /// As in `set name = "Use"`.
        Assign,
        /// As in `set src in usr`.
        In,
    }
}