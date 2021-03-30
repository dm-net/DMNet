using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class BlockConverter : CborConverterBase<Block>
    {
        public override Block Read(ref CborReader reader)
        {
            var options = new CborOptions();
            ICborConverter<Spaned<Statement>[]> objectConverter = options.Registry.ConverterRegistry.Lookup<Spaned<Statement>[]>();
            return new Block()
            {
                Value = objectConverter.Read(ref reader)
            };
        }

        public static Block ParseBlock(CborValue element)
        {
            var cborArray = (CborArray)element;
            return new Block()
            {
                Value = cborArray.ToCollection<Spaned<Statement>[]>()
            };
        }
    }
}
