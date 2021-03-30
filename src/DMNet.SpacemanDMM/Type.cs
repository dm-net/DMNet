using Dahomey.Cbor;
using DMNet.SpacemanDMM.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM
{
    public struct Type
    {
        /// <summary>
        /// The reference.
        /// </summary>
        private readonly IntPtr reference;

        public bool IsValid => reference != IntPtr.Zero;
        private void Verify()
        {
            if (!IsValid)
                throw new NullReferenceException();
        }


        public string Path
        {
            get
            {
                Verify();
                var ptr = Native.dreammaker_type_getpath(this);
                var str = Utf8String.FromIntPtr(ptr);
                Native.str_free(ptr);
                return str;
            }
        }

        public bool IsRoot
        {
            get
            {
                Verify();
                return Native.dreammaker_type_isroot(this);
            }
        }

        public List<Type> GetChildren()
        {
            var list = new List<Type>();
            Verify();
            Native.dreammaker_type_iterchildren(this, type => list.Add(type));
            return list;
        }

        public byte[] GetVarsCbor()
        {
            Verify();
            var cbor = Native.dreammaker_type_varscbor(this);
            var data = cbor.GetBytes();
            cbor.Free();
            return data;
        }
        
        public TypeVar[] GetVars()
        {
            return Cbor.Deserialize<TypeVar[]>(GetVarsCbor());
        }

        public byte[] GetProcsCbor()
        {
            Verify();
            var cbor = Native.dreammaker_type_procscbor(this);
            var data = cbor.GetBytes();
            cbor.Free();
            return data;
        }

        public TypeProc[] GetProcs()
        {
            return Cbor.Deserialize<TypeProc[]>(GetProcsCbor());
        }

        public void Free()
        {
            Verify();
            Native.dreammaker_type_free(this);
        }
    }
}
