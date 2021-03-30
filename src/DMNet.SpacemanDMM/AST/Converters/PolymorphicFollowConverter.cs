﻿using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using Dahomey.Cbor.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.AST.Converters
{
    class PolymorphicFollowConverter : CborConverterBase<Follow>
    {
        public override Follow Read(ref CborReader reader)
        {
            var conv = new CborValueConverter(new CborOptions());
            var cborValue = conv.Read(ref reader);
            return ParseFollow(cborValue);
        }

        public static Follow ParseFollow(CborValue element)
        {
            if (element == null || element.Type == CborValueType.Null)
                return null;
            var obj = (CborObject)element;
            foreach (var prop in obj)
            {
                var cborArray = (CborArray)prop.Value;
                switch (prop.Key.Value<string>())
                {
                    case "Field":
                        return new Follow.Field()
                        {
                            Index = Enum.Parse<Follow.IndexKind>(cborArray[0].Value<string>()),
                            Identifier = cborArray[1].Value<string>()
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
