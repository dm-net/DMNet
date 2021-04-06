using DMNet.SpacemanDMM.AST;
using DMType = DMNet.SpacemanDMM.Type;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMNet.Compiler.Extensions
{
    public static class AST
    {
        private static IEnumerable<E> GetFlags<E>(E input) where E : Enum
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return (E)value;
        }

        public static FieldAttributes DetermineBestFieldAttributes(this TypeVar typeVar)
        {
            FieldAttributes attributes = FieldAttributes.Public;
            if (typeVar.Declaration == null)
                return attributes;
            foreach (var flag in GetFlags(typeVar.Declaration.Type.Flags))
            {
                switch (flag)
                {
                    case VarTypeFlags.STATIC:
                        attributes |= FieldAttributes.Static;
                        break;
                    case VarTypeFlags.CONST:
                        if(typeVar.CanHaveDefaultValue())
                            attributes |= FieldAttributes.Literal;
                        else
                            attributes |= FieldAttributes.InitOnly;
                        break;
                    case VarTypeFlags.TMP:
                        break;
                    case VarTypeFlags.FINAL:
                        attributes |= FieldAttributes.InitOnly;
                        break;
                    case VarTypeFlags.PRIVATE:
                        attributes ^= FieldAttributes.Public;
                        attributes |= FieldAttributes.Private;
                        break;
                    case VarTypeFlags.PROTECTED:
                        attributes ^= FieldAttributes.Public;
                        attributes |= FieldAttributes.Family;
                        break;
                    default:
                        break;
                }
            }
            return attributes;
        }

        public static bool CanHaveDefaultValue(this TypeVar value)
        {
            switch (value.Value.Constant)
            {
                case Constant.Float _:
                case Constant.Int _:
                case Constant.Null _:
                case Constant.String _:
                    return true;
                default:
                    return false;
            }
        }

        public static Type TypeOf(this TypeVar value)
        {
            switch (value.Value.Constant)
            {
                case Constant.Float _:
                    return typeof(float);
                case Constant.Int _:
                    return typeof(int);
                case Constant.Null _:
                    return typeof(object);
                case Constant.String _:
                    return typeof(string);
            }
            // Use other methods to determine type.

            // TODO: use other methods

            // Fallback to Object
            return typeof(object);
        }

        public static Symbol.TypeSymbol ParentTypeSymbol(this DMType value, SymbolTable globalSymbols)
        {
            var parentType = value.ParentType;
            if (string.IsNullOrEmpty(parentType?.Path))
                return null; // ToDo: This should not be reached

            return (Symbol.TypeSymbol)globalSymbols[new SymbolKey(SymbolType.Type, parentType.Path)];
        }
    }
}
