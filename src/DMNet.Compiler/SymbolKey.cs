using System;
using System.Collections.Generic;
using System.Text;

namespace DMNet.Compiler
{
    public struct SymbolKey : IEquatable<SymbolKey>
    {
        public string Name;
        public SymbolType Type;

        public SymbolKey(SymbolType type, string name)
        {
            Name = name;
            Type = type;
        }

        public bool Equals(SymbolKey other)
        {
            return Type.Equals(other.Type) && Name.Equals(other.Name);
        }

        public static SymbolKey Ctor => new SymbolKey(SymbolType.Ctor, ".ctor");

        public override string ToString()
        {
            return $"({Type}-{Name})";
        }
    }

    public enum SymbolType
    {
        Ctor,
        Type,
        Proc,
        Var,
        Scope
    }
}
