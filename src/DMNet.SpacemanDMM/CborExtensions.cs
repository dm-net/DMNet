using Dahomey.Cbor.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM
{
    public static class CborExtensions
    {

        public static T ToObject<T>(this CborValue value) where T : class
        {
            if (value == null || value.Type == CborValueType.Null)
                return null;
            if (value is CborObject valObj)
                return valObj.ToObject<T>();
            return null;
        }

        public static T ToCollection<T>(this CborValue value) where T : System.Collections.ICollection
        {
            if (value == null || value.Type == CborValueType.Null)
                return default(T);
            if (value is CborArray valArr)
                return valArr.ToCollection<T>();
            return default(T);
        }
    }
}
