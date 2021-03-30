using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM
{
    public class ManagedParser : IDisposable
    {
        ObjectTree objectTree;
        Context context;
        private bool disposedValue;

        public ManagedParser(string path)
        {
            var str = new Utf8String(path);
            Native.dreammaker_load(str.IntPtr, (tree, ctx) =>
            {
                objectTree = tree;
                context = ctx;
            });
        }

        public Type RootType => objectTree.Root;

        public List<Type> GetTypes() => objectTree.GetTypes();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                Native.dreammaker_unload(objectTree, context);
                
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~ManagedParser()
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
