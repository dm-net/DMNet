using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicCodeConverter : CborConverterBase<Code>
    {
        public override Code Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseCode(cborValue);
        }

        public static Code ParseCode(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            if (element is CborString str)
            {
                switch (str.Value<string>())
                {
                    case "Builtin":
                        return new Code.Builtin();
                    case "Disabled":
                        return new Code.Disabled();
                    default:
                        throw new Exception();
                }
            }
            else if (element is CborObject obj)
            {
                foreach (var prop in obj)
                {
                    switch (prop.Key.Value<string>())
                    {
                        case "Present":
                            return new Code.Present()
                            {
                                Block = BlockConverter.ParseBlock(prop.Value)
                            };
                        default:
                            Console.WriteLine($"{prop.Key}: {prop.Value}");
                            throw new Exception();
                    }
                }
            }
            else
            {
                throw new Exception();
            }
            return null;
        }
    }
}
