using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicCaseConverter : CborConverterBase<Case>
    {
        public override Case Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseCase(cborValue);
        }

        public static Case ParseCase(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                var valArr = prop.Value as CborArray;
                switch (prop.Key.Value<string>())
                {
                    case "Exact":
                        return new Case.Exact()
                        {
                            Value = PolymorphicExpressionConverter.ParseExpression(prop.Value),
                        };
                    case "Range":
                        return new Case.Range()
                        {
                            From = PolymorphicExpressionConverter.ParseExpression(valArr[0]),
                            To = PolymorphicExpressionConverter.ParseExpression(valArr[1])
                        };
                    default:
                        Console.WriteLine($"{prop.Key}: {prop.Value}");
                        throw new Exception();
                }
            }
            return null;
        }
    }
}
