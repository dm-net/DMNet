using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM
{
    public struct Var
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

        public void Free()
        {
            Verify();
            Native.dreammaker_var_free(this);
        }
    }
}
