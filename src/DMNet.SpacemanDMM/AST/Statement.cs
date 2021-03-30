using Dahomey.Cbor.Attributes;
using DMNet.SpacemanDMM.AST.Converters;

namespace DMNet.SpacemanDMM.AST
{
    [CborConverter(typeof(PolymorphicStatementConverter))]
    public class Statement
    {
        // Expr

        // Return

        // Throw

        // While

        // DoWhile

        // If

        // ForLoop

        // ForList

        // ForRange

        // Var

        // Vars

        // Setting

        // Spawn

        // Switch

        // TryCatch

        // Continue

        // Break

        // Goto

        // Label

        // Del

        // Crash
    }
}