using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM
{
    internal struct ObjectTree
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

        public Type Root
        {
            get
            {
                Verify();
                return Native.dreammaker_objecttree_root(this);
            }
        }

        public List<Type> GetTypes()
        {
            var list = new List<Type>();
            Verify();
            Native.dreammaker_objecttree_itertypes(this, type => list.Add(type));
            return list;
        }
    }
}
