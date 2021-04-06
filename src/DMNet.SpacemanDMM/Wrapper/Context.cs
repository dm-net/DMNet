using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM.Wrapper
{
    internal struct Context
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
    }
}
