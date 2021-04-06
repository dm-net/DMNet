using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class FlagConverter<T> : CborConverterBase<T> where T: Enum
    {
        public override T Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseFlag(cborValue);
        }

        public static T ParseFlag(CborValue value)
        {
            if (value == null || value.Type == CborValueType.Null)
                return default(T);
            var cborObject = (CborObject)value;
            return (T)Enum.ToObject(typeof(T), cborObject["bits"].Value<int>());
        }
    }
}
