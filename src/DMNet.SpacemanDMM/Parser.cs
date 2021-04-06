using DMNet.SpacemanDMM.Wrapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DMNet.SpacemanDMM
{
    public class Parser : IDisposable
    {
        ObjectTree objectTree;
        Context context;
        private bool disposedValue;

        private Type[] types;
        private Dictionary<uint, Type> indexedTypes = new Dictionary<uint, Type>();

        public Type[] Types => types;


        public Parser(string path)
        {
            var str = new Utf8String(path);
            Native.dreammaker_load(str.IntPtr, (tree, ctx) =>
            {
                objectTree = tree;
                context = ctx;
            });
            LoadTypes();
            Native.dreammaker_unload(objectTree, context);
        }

        private void LoadTypes()
        {
            types = objectTree.GetTypes().Select((t) => new Type(t, this)).ToArray();
        }


        internal void RegisterIndexedType(Type type, uint index)
        {
            indexedTypes.Add(index, type);
        }

        internal Type GetTypeByIndex(uint index)
        {
            if (index == uint.MaxValue)
                return null;
            if (!indexedTypes.ContainsKey(index))
                throw new Exception("Provided index is invalid.");
            return indexedTypes[index];
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                // Native.dreammaker_unload(objectTree, context);
                
                disposedValue = true;
            }
        }


        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Parser()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
