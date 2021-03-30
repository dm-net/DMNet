using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PopConverter : CborConverterBase<Pop>
    {
        public override Pop Read(ref CborReader reader)
        {
            ICborConverter<CborObject> conv = new CborValueConverter(new CborOptions());
            var cborObject = conv.Read(ref reader);
            return ParsePop(cborObject);
        }

        public static Pop ParsePop(CborValue element)
        {
            if (element.Type == CborValueType.Null)
                return null;
            CborObject obj;
            if (element is CborObject cobj)
                obj = cobj;
            else
                obj = element.Value<CborObject>();

            var p = new List<string>();
            foreach (var item in (CborArray)obj["path"])
            {
                p.Add(item.Value<string>());
            }
            var pop = new Pop()
            {
                Path = p.ToArray()
            };

            var vars = (CborArray)obj["vars"];
            foreach (var tuple in vars)
            {
                var arrTuple = (CborArray)tuple;
                pop.Vars[arrTuple[0].Value<string>()] = PolymorphicConstantConverter.ParseConstant(arrTuple[1]);
            }

            return pop;
        }
    }
}
