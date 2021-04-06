using DMNet.SpacemanDMM.AST;
using DMNet.Compiler.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DMType = DMNet.SpacemanDMM.Type;
using System.Collections.Generic;
using System.Text;

namespace DMNet.Compiler
{
    public partial class Compiler
    {
        private TypeBuilder glob;


        public Assembly Emit(string name)
        {
            var aName = new AssemblyName(name);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name);
            // Define stage
            define(moduleBuilder, name);

            // Emit stage
            // ToDo: loop over each type and emit non constant var value setting.
            // ToDo: loop over each proc definition and emit code

            // End stage
            finalizeTypes(moduleBuilder, name);

            return assemblyBuilder;
        }

        private void define(ModuleBuilder moduleBuilder, string assemblyName)
        {
            defineTypes(moduleBuilder, assemblyName);
            defineGlobals(moduleBuilder, assemblyName);
            defineTypesVarsAndProcs();
        }

        private void defineTypesVarsAndProcs()
        {
            var types = globalSymbols.Where(kv => kv.Key.Type == SymbolType.Type).Select(kv => (Symbol.TypeSymbol)kv.Value);

            var pendingDefinitions = types.ToList();
            while (pendingDefinitions.Count > 0)
            {
                var nextBatch = new List<Symbol.TypeSymbol>();
                foreach (var type in pendingDefinitions)
                {
                    if (type.Definition != null)
                    {
                        var parrentTypeSymbol = type.Type.ParentTypeSymbol(globalSymbols);
                        if (parrentTypeSymbol == null || parrentTypeSymbol.IsFullyDefined)
                        {
                            foreach (var item in type.Symbols)
                            {
                                switch (item.Key.Type)
                                {
                                    case SymbolType.Proc:
                                        //defineProc(glob, item.Key.Name, (Symbol.ProcSymbol)item.Value);
                                        break;
                                    case SymbolType.Var:
                                        defineTypeVar(type.Definition, item.Key.Name, (Symbol.VarSymbol)item.Value);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            type.IsFullyDefined = true;
                        } else
                        {
                            nextBatch.Add(type);
                        }
                    }
                }
                pendingDefinitions = nextBatch;
            }
        }



        private void defineTypes(ModuleBuilder moduleBuilder, string assemblyName)
        {
            var types = globalSymbols.Where(kv => kv.Key.Type == SymbolType.Type).Select(kv => (Symbol.TypeSymbol)kv.Value);

            var postponedTypes = types.ToList();


            while (postponedTypes.Count > 0)
            {
                var newPostponed = new List<Symbol.TypeSymbol>();
                foreach (var type in postponedTypes)
                {
                    
                    switch (type.Type.Path)
                    {
                        case "/datum":
                            {
                                string className = GenerateClassNameFromPath(type.Type.Path, assemblyName);
                                var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(Runtime.DatumBase));
                                type.Definition = typeBuilder;
                            }
                            break;
                        case "/list":
                            break; // Fuck lists
                        case "/world":
                            {
                                string className = GenerateClassNameFromPath(type.Type.Path, assemblyName);
                                var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(Runtime.WorldBase));
                                type.Definition = typeBuilder;
                            }
                            break;
                        case "/client":
                            {
                                string className = GenerateClassNameFromPath(type.Type.Path, assemblyName);
                                var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(Runtime.ClientBase));
                                type.Definition = typeBuilder;
                            }
                            break;
                        case "/savefile":
                            {
                                string className = GenerateClassNameFromPath(type.Type.Path, assemblyName);
                                var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(Runtime.SaveFileBase));
                                type.Definition = typeBuilder;
                            }
                            break;
                        default:
                            // Get parent type symbol
                            var parentType = type.Type.ParentTypeSymbol(globalSymbols);
                            if(parentType.Definition == null)
                            {
                                newPostponed.Add(type);
                            } else
                            {
                                string className = GenerateClassNameFromPath(type.Type.Path, assemblyName);
                                var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, parentType.Definition);

                                type.Definition = typeBuilder;
                            }
                            break;
                    }
                }
                postponedTypes = newPostponed;
            }
        }

        private void defineGlobals(ModuleBuilder moduleBuilder, string assemblyName)
        {
            glob = moduleBuilder.DefineType($"{assemblyName}.Global", TypeAttributes.Public | TypeAttributes.Sealed, typeof(Runtime.GlobalBase));

            foreach (var item in globalSymbols)
            {
                switch (item.Key.Type)
                {
                    case SymbolType.Proc:
                        //defineProc(glob, item.Key.Name, (Symbol.ProcSymbol)item.Value);
                        break;
                    case SymbolType.Var:
                        defineTypeVar(glob, item.Key.Name, (Symbol.VarSymbol)item.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        private void defineTypeVar(TypeBuilder typeBuilder, string name, Symbol.VarSymbol varSymbol)
        {
            if (varSymbol.Var == null)
                throw new Exception("Locals should not be defined in define stage.");
            if (varSymbol.Var.Name == "parent_type")
                return; // parent_type is special compile time var

            var baseVarSymbol = varSymbol.FindVarBaseDefinition(globalSymbols);
            if(baseVarSymbol == varSymbol)
            {
                Type varType = varSymbol.Var.TypeOf();
                FieldAttributes attributes = varSymbol.Var.DetermineBestFieldAttributes();
                var fieldBuilder = typeBuilder.DefineField(name, varType, attributes);
                if (varSymbol.Var.CanHaveDefaultValue())
                {
                    switch (varSymbol.Var.Value.Constant)
                    {
                        case Constant.Int val:
                            fieldBuilder.SetConstant(val.Value);
                            break;
                        case Constant.Float val:
                            fieldBuilder.SetConstant(val.Value);
                            break;
                        case Constant.String val:
                            fieldBuilder.SetConstant(val.Value);
                            break;
                        case Constant.Null _:
                            fieldBuilder.SetConstant(null);
                            break;
                        default:
                            throw new NotImplementedException("This code should not be reached.");
                    }
                }
                varSymbol.Definition = fieldBuilder;
            } else
            {
                varSymbol.Definition = baseVarSymbol.Definition; // We borow definition from initial var declaration place
            }
        }

        private void defineProc(TypeBuilder typeBuilder, string name, Symbol.ProcSymbol procSymbol)
        {
            // ToDo: determine return type
            var procBuilder = typeBuilder.DefineMethod(name, MethodAttributes.Public);
            procSymbol.Definition = procBuilder;
        }

        private void finalizeTypes(ModuleBuilder mb, string assemblyName)
        {
            glob.CreateType();
            foreach (var item in globalSymbols.Where(kv => kv.Key.Type == SymbolType.Type).Select(kv => (Symbol.TypeSymbol)kv.Value))
            {
                item.Definition?.CreateType();
            }
        }


        private static string GenerateClassNameFromPath(string path, string assemblyName)
        {
            var sb = new StringBuilder(assemblyName);
            sb.Append('.');
            foreach (var segment in path.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                sb.Append(char.ToUpper(segment[0]));
                sb.Append(segment.Substring(1));
                sb.Append('.');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
