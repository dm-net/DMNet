using DMNet.SpacemanDMM.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.SpacemanDMM
{
    public class Type
    {
        private Wrapper.Type type;

        private TypeProc[] _procs = null;
        private TypeVar[] _vars = null;
        private string _typePath = null;
        private Parser _parser;
        private uint _index;
        private uint _parentIndex;

        public TypeProc[] Procs => _procs;
        public TypeVar[] Vars => _vars;

        public string Path => _typePath;

        public bool IsRoot => string.IsNullOrEmpty(Path);

        public Type ParentType => _parser.GetTypeByIndex(_parentIndex);

        internal Type(Wrapper.Type nativeType, Parser parser)
        {
            _parser = parser;
            type = nativeType;
            load();
            type.Free();
            type = Wrapper.Type.Invalid;
        }

        private void load()
        {
            if (_procs != null)
                throw new Exception();
            _procs = type.GetProcs();
            if (_vars != null)
                throw new Exception();
            _vars = type.GetVars();
            _typePath = type.Path;
            _index = type.Index;
            _parentIndex = type.ParentIndex;
            _parser.RegisterIndexedType(this, _index);
        }

        public TypeVar GetVar(string name)
        {
            foreach (var item in _vars)
                if (item.Name.Equals(name))
                    return item;
            return null;
        }
    }
}
