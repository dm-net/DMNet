using DMNet.Compiler.Extensions;
using DMNet.SpacemanDMM.AST;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DMNet.Compiler
{
    public abstract class Symbol
    {
        public Symbol Parent { get; set; }
        public SymbolTable Holder { get; set; }

        public class ScopeSymbol : Symbol
        {
            public SymbolTable Symbols { get; set; }

            public ScopeSymbol()
            {
                Symbols = new SymbolTable(this);
            }
        }

        public class StatementSymbol : ScopeSymbol
        {
            public Statement Statement { get; set; }

            public StatementSymbol(Statement statement) : base()
            {
                Statement = statement;
            }
        }

        public class VarSymbol : Symbol
        {
            public TypeVar Var { get; set; }
            public VarStatement VarStatement { get; set; }

            public DefinitionStage Stage { get; set; }

            private FieldInfo field;

            public FieldInfo Definition { 
                get
                {
                    return field;
                }
                internal set
                {
                    field = value;
                }
            }
            public FieldBuilder DefinitionBuilder { 
                get
                {
                    return field as FieldBuilder;
                }
                internal set
                {
                    field = value;
                }
            }

            public VarSymbol() : base() { }
            public VarSymbol(TypeVar variable) : base()
            {
                Var = variable;
            }

            public VarSymbol(VarStatement variable) : base()
            {
                VarStatement = variable;
            }

            public VarSymbol FindVarBaseDefinition(SymbolTable globalSymbols)
            {
                if (Var == null)
                    throw new Exception("I can't do this for local vars.");
                if (Parent == null)
                    return this;

                var myType = (TypeSymbol)Parent; // I expect to see my type here;

                var myKey = new SymbolKey(SymbolType.Var, Var.Name);

                var bestDefinition = this;
                var currType = myType.Type.ParentTypeSymbol(globalSymbols);
                while(currType != null)
                {
                    currType.Symbols.TryGetValue(myKey, out var potential);
                    if (potential != null)
                        bestDefinition = (VarSymbol)potential;

                    currType = currType.Type?.ParentTypeSymbol(globalSymbols);
                }
                return bestDefinition;
            }
        }

        public class ProcSymbol : ScopeSymbol
        {
            public TypeProc Proc { get; set; }
            private MethodInfo method;

            public MethodInfo Definition
            {
                get
                {
                    return method;
                }
                internal set
                {
                    method = value;
                }
            }
            public MethodBuilder DefinitionBuilder
            {
                get
                {
                    return method as MethodBuilder;
                }
                internal set
                {
                    method = value;
                }
            }

            public DefinitionStage Stage { get; set; }

            public ProcSymbol(TypeProc proc) : base()
            {
                Proc = proc;
            }

            public ProcSymbol() : base() { }
        }

        public class CtorSymbol : Symbol
        {
            private ConstructorInfo constructor;

            public ConstructorInfo Definition
            {
                get
                {
                    return constructor;
                }
                internal set
                {
                    constructor = value;
                }
            }
            public ConstructorBuilder DefinitionBuilder
            {
                get
                {
                    return constructor as ConstructorBuilder;
                }
                internal set
                {
                    constructor = value;
                }
            }

            public DefinitionStage Stage { get; set; }
        }

        public class TypeSymbol : ScopeSymbol
        {
            public SpacemanDMM.Type Type { get; set; }
            public DefinitionStage Stage { get; set; }
            private Type definedType;

            public Type Definition {
                get
                {
                    return definedType;
                }
                internal set
                {
                    definedType = value;
                }
            }
            public TypeBuilder DefinitionBuilder {
                get
                {
                    return definedType as TypeBuilder;
                }
                internal set
                {
                    definedType = value;
                }
            }

            // Has been it's vars and procs defined
            public bool IsFullyDefined { get; set; } = false;

            public TypeSymbol(SpacemanDMM.Type type) : base()
            {
                Type = type;
            }

            public TypeSymbol() : base() { }
        }

        public enum DefinitionStage
        {
            UNDEFINED,
            SIGNATURE,
            VALUE,
            FULLY_DEFINED,
        }
    }
}
