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
            base.Add(key, value);
        }

        public void AddAnonimousScope(Symbol symbol)
        {
            Add(new SymbolKey(SymbolType.Scope, $"<anon{anonCounter}>") , symbol);
            anonCounter++;
        }
    }
}
