using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST
{
    public static class Operators
    {
        public enum UnaryOp
        {
            Neg,
            Not,
            BitNot,
            PreIncr,
            PostIncr,
            PreDecr,
            PostDecr,
        }

        public enum PathOp
        {
            /// `/` for absolute pathing.
            Slash,
            /// `.` for checked relative pathing.
            Dot,
            /// `:` for unchecked relative pathing.
            Colon,
        }

        public enum BinaryOp
        {
            Add,
            Sub,
            Mul,
            Div,
            Pow,
            Mod,
            Eq,
            NotEq,
            Less,
            Greater,
            LessEq,
            GreaterEq,
            Equiv,
            NotEquiv,
            BitAnd,
            BitXor,
            BitOr,
            LShift,
            RShift,
            And,
            Or,
            In,
            To,  // only appears in RHS of `In`
        }

        public enum AssignOp
        {
            Assign,
            AddAssign,
            SubAssign,
            MulAssign,
            DivAssign,
            ModAssign,
            AssignInto,
            BitAndAssign,
            AndAssign,
            BitOrAssign,
            OrAssign,
            BitXorAssign,
            LShiftAssign,
            RShiftAssign,
        }

        public enum TernaryOp
        {
            Conditional,
        }
    }


    
}
