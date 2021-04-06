using System;
using System.Runtime.InteropServices;

namespace DMNet.SpacemanDMM.Wrapper
{
    internal static class Native
    {
        public const string Dreammaker_ffiLib = "dreammaker";

        #region delegates
        internal delegate void TypeIterator(Type type);
        internal delegate void OnLoadedCallback(ObjectTree tree, Context context);
        #endregion


        [DllImport(Dreammaker_ffiLib)]
        internal static extern void dreammaker_load(IntPtr path, OnLoadedCallback loadedCallback);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern void dreammaker_unload(ObjectTree objectTree, Context context);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern Type dreammaker_objecttree_root(ObjectTree objectTree);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern void dreammaker_objecttree_itertypes(ObjectTree objectTree, TypeIterator iterator);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern void dreammaker_type_free(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern bool dreammaker_type_isroot(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern IntPtr dreammaker_type_getpath(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern uint dreammaker_type_getindex(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern uint dreammaker_type_getparentindex(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern void dreammaker_type_iterchildren(Type typeref, TypeIterator iterator);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern CborData dreammaker_type_varscbor(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern CborData dreammaker_type_procscbor(Type typeref);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern void dreammaker_cbor_free(CborData cborData);

        [DllImport(Dreammaker_ffiLib)]
        internal static extern void str_free(IntPtr str);
    }
}

