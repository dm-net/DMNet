using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicConstantConverter : CborConverterBase<Constant>
    {
        public override Constant Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseConstant(cborValue);
        }

        public static Constant ParseConstant(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                switch (prop.Key.Value<string>())
                {
                    case "Null":
                        return new Constant.Null();
                    case "New": // {"type_":{"path":["list"],"vars":[]},"args":[[{"Int":0},null]]}
                        var o = (CborObject)prop.Value;
                        var pop = PopConverter.ParsePop(o["type_"]);
                        var args = parseList(o["args"]);
                        return new Constant.New()
                        {
                            Type = pop,
                            Args = args
                        };
                    case "List":
                        return parseList(prop.Value);
                    case "Call": // ["Icon",[[{"Resource":"icons/obj/drinks.dmi"},null],[{"String":"broken"},null]]]
                        var a = (CborArray)prop.Value;
                        return new Constant.Call()
                        {
                            Function = Enum.Parse<Constant.ConstFn>(a[0].Value<string>()),
                            Args = parseList(a[1])
                        };
                    case "Prefab":
                        return new Constant.Prefab()
                        {
                            Value = PopConverter.ParsePop(prop.Value)
                        };
                    case "String":
                        return new Constant.String()
                        {
                            Value = prop.Value.Value<string>()
                        };
                    case "Resource":
                        return new Constant.Resource()
                        {
                            Value = prop.Value.Value<string>()
                        };
                    case "Int":
                        return new Constant.Int()
                        {
                            Value = prop.Value.Value<int>()
                        };
                    case "Float":
                        return new Constant.Float()
                        {
                            Value = prop.Value.Value<float>()
                        };
                    default:
                        Console.WriteLine($"{prop.Key}: {prop.Value}");
                        throw new Exception();
                }
            }
            return null;
        }

        private static Constant.List parseList(CborValue element)
        {
            if (element.Type == CborValueType.Null || element == null)
                return null;
            var arr = (CborArray)element;
            var constList = new Constant.List();
            foreach (var item in arr)
            {
                var tuple = new ValueTuple<Constant, Constant>(null, null);
                var tarr = (CborArray)item;
                tuple.Item1 = ParseConstant(tarr[0]);
                tuple.Item2 = ParseConstant(tarr[1]);
                constList.Items.Add(tuple.ToTuple());
            }
            return constList;
        }
    }
}
