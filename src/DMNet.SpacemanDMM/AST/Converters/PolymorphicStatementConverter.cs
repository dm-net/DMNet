using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicStatementConverter : CborConverterBase<Statement>
    {
        public override Statement Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseStatement(cborValue);
        }

        public static Statement ParseStatement(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                switch (prop.Key.Value<string>())
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
                    default:
                        Console.WriteLine($"{prop.Key}: {prop.Value}");
                        throw new Exception();
                }
            }
            return null;
        }
    }
}
