using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PrefabConverter : CborConverterBase<Prefab>
    {
        public override Prefab Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParsePrefab(cborValue);
        }

        public static Prefab ParsePrefab(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var prefabObj = (CborObject)element;
            var prefabPath = (CborArray)prefabObj["path"];
            var prefabPathList = new List<Tuple<Operators.PathOp, string>>();
            foreach (var item in prefabPath)
            {
                var rawTuple = (CborArray)item;
                prefabPathList.Add(new Tuple<Operators.PathOp, string>(Enum.Parse<Operators.PathOp>(rawTuple[0].Value<string>()), rawTuple[1].Value<string>()));
            }
            var prefab = new Prefab()
            {
                Path = prefabPathList.ToArray()
            };
            var prefabVars = (CborArray)prefabObj["vars"];
            foreach (var item in prefabVars)
            {
                var rawTuple = (CborArray)item;
                prefab.Vars[rawTuple[0].Value<string>()] = PolymorphicExpressionConverter.ParseExpression(rawTuple[1]);
            }
            return prefab;
        }
    }
}
