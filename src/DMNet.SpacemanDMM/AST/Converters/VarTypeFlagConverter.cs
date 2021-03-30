using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class VarTypeFlagConverter : CborConverterBase<VarTypeFlags>
    {
        public override VarTypeFlags Read(ref CborReader reader)
        {
            ICborConverter<CborObject> conv = new CborValueConverter(new CborOptions());
            var cborObject = conv.Read(ref reader);
            return (VarTypeFlags)cborObject["bits"].Value<int>();
        }
    }
}
