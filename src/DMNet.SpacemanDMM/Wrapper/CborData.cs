using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DMNet.SpacemanDMM.Wrapper
{
    [StructLayout(LayoutKind.Sequential)]
    struct CborData
    {
        IntPtr Data;
        int Length;

        public byte[] GetBytes()
        {
            var bytes = new byte[Length];
            Marshal.Copy(Data, bytes, 0, Length);
            return bytes;
        }

        public void Free()
        {
            Native.dreammaker_cbor_free(this);
        }
    }
}
