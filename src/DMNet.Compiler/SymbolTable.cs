using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.Compiler
{
    public class SymbolTable : Dictionary<SymbolKey, Symbol>
    {
        private Symbol _holder;

        private int anonCounter = 0;

        public SymbolTable(Symbol holder)
        {
            _holder = holder;
        }

        public new void Add(SymbolKey key, Symbol value)
        {
            value.Parent = _holder;
            value.Holder = this;
            base.Add(key, value);
        }

        public void AddAnonimousScope(Symbol symbol)
        {
            Add(new SymbolKey(SymbolType.Scope, $"<anon{anonCounter}>") , symbol);
            anonCounter++;
        }

        public Symbol LookupTypePath(string[] typePath)
        {
            if(_holder != null)
            {
                // Do look up on root SymbolTable
                return _holder.Holder.LookupTypePath(typePath);
            }

            var path = '/' + string.Join('/', typePath);

            var symbol = this[new SymbolKey(SymbolType.Type, path)];

            return symbol;
        }
    }
}
