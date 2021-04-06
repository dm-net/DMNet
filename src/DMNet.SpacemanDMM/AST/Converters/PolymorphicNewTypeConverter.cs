using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicNewTypeConverter : CborConverterBase<NewType>
    {
        public override NewType Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseNewType(cborValue);
        }

        public static NewType ParseNewType(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            if (element is CborString str)
            {
                switch (str.Value<string>())
                {
                    case "Implicit":
                        return new NewType.Implicit();
                    default:
                        throw new Exception();
                }
            } else if (element is CborObject obj) {
                foreach (var prop in obj)
                {
                    switch (prop.Key.Value<string>())
                    {
                        case "Prefab":
                            return new NewType.Prefab()
                            {
                                Value = PrefabConverter.ParsePrefab(prop.Value)
                            };
                        case "MiniExpr":
                            var valObj = (CborObject)prop.Value;
                            return new NewType.MiniExpr()
                            {
                                Identifier = valObj["ident"].Value<string>(),
                                Fields = ((CborArray)valObj["fields"]).ToCollection<Follow[]>()
                            };
                        default:
                            Console.WriteLine($"{prop.Key}: {prop.Value}");
                            throw new Exception();
                    }
                }
            } else
            {
                throw new Exception();
            }
            return null;
        }
    }
}
