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
            EmitVars();
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
                            defineTypeVarsAndProcs(type);
                        } else
                        {
                            nextBatch.Add(type);
                        }
                    }
                }
                pendingDefinitions = nextBatch;
            }
        }

        private void defineTypeVarsAndProcs(Symbol.TypeSymbol type)
        {
            foreach (var item in type.Symbols)
            {
                switch (item.Key.Type)
                {
                    case SymbolType.Proc:
                        defineProc(glob, item.Key.Name, (Symbol.ProcSymbol)item.Value);
                        break;
                    case SymbolType.Var:
                        defineTypeVar(type.DefinitionBuilder, item.Key.Name, (Symbol.VarSymbol)item.Value);
                        break;
                    default:
                        break;
                }
            }
            type.IsFullyDefined = true;
        }

        private void EmitVars()
        {
            // Emit globals
            _emitVars((Symbol.CtorSymbol)globalSymbols[SymbolKey.Ctor], globalSymbols);

            var types = globalSymbols.Where(kv => kv.Key.Type == SymbolType.Type).Select(kv => (Symbol.TypeSymbol)kv.Value).Where(i => i.Stage != Symbol.DefinitionStage.FULLY_DEFINED);
            foreach (var item in types)
            {
                _emitVars((Symbol.CtorSymbol)item.Symbols[SymbolKey.Ctor], item.Symbols);
            }
        }

        private void _emitVars(Symbol.CtorSymbol ctorSymbol, SymbolTable emitededVars)
        {
            var ilWriter = ctorSymbol.DefinitionBuilder.GetILGenerator();
            foreach (var variable in emitededVars.Where(kv => kv.Key.Type == SymbolType.Var).Select(kv => (Symbol.VarSymbol)kv.Value).Where(i => i.Stage != Symbol.DefinitionStage.FULLY_DEFINED && i.Stage != Symbol.DefinitionStage.UNDEFINED))
            {
                if (!variable.Definition.Attributes.HasFlag(FieldAttributes.Static))
                {
                    ilWriter.Emit(OpCodes.Ldarg_0);
                }

                if (variable.Var.Value.Constant != null)
                {
                    _emit(variable.Var.Value.Constant, ilWriter);
                } else
                {
                    _emit(variable.Var.Value.Expression, ilWriter, true);
                }
                

                if (variable.Definition.Attributes.HasFlag(FieldAttributes.Static))
                    // TODO: Move this to static constructor.
                    ilWriter.Emit(OpCodes.Stsfld, variable.Definition);
                else
                {
                    ilWriter.Emit(OpCodes.Stfld, variable.Definition);
                }
                    
            }

        }

        private void _emit(Constant constant, ILGenerator iLGen)
        {
            switch (constant)
            {
                case Constant.Int intConst:
                    iLGen.Emit(OpCodes.Ldc_I4, intConst.Value);
                    break;

                case Constant.Prefab prefabConst:
                    // TODO
                    if (prefabConst.Value.Vars.Count > 0)
                        throw new NotImplementedException();
                    var sym = globalSymbols.LookupTypePath(prefabConst.Value.Path);
                    switch (sym)
                    {
                        case Symbol.TypeSymbol typeSymbol:
                            iLGen.Emit(OpCodes.Ldtoken, typeSymbol.Definition);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    //throw NotImplementedException();
                    iLGen.Emit(OpCodes.Ldnull);
                    break;
            }
        }

        private void _emit(Expression expression, ILGenerator iLGen, bool leaveValue = true)
        {
            switch (expression)
            {
                case null:
                    if(leaveValue)
                        iLGen.Emit(OpCodes.Ldnull);
                    break;
                case Expression.Base baseExp:
                    // TODO 
                    _emit(baseExp.Term.Value, iLGen);
                    break;
                default:
                    //throw NotImplementedException();
                    iLGen.Emit(OpCodes.Ldnull);
                    break;
            }
            
        }

        private void _emit(Term term, ILGenerator iLGen)
        {
            switch (term)
            {
                case Term.Int intTerm:
                    iLGen.Emit(OpCodes.Ldc_I4, intTerm.Value);
                    break;
                case Term.Float floatTerm:
                    iLGen.Emit(OpCodes.Ldc_R4, floatTerm.Value);
                    break;
                default:
                    //throw NotImplementedException();
                    iLGen.Emit(OpCodes.Ldnull);
                    break;
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
                    if (type.Stage == Symbol.DefinitionStage.FULLY_DEFINED)
                        continue;
                    var parentType = type.Type.ParentTypeSymbol(globalSymbols);
                    if (parentType.Definition == null)
                    {
                        newPostponed.Add(type);
                    }
                    else
                    {
                        defineType(moduleBuilder, assemblyName, type, parentType);
                    }
                }
                postponedTypes = newPostponed;
            }
        }

        private void defineType(ModuleBuilder moduleBuilder, string assemblyName, Symbol.TypeSymbol typeSymbol, Symbol.TypeSymbol parentTypeSymbol)
        {
            string className = GenerateClassNameFromPath(typeSymbol.Type.Path, assemblyName);
            var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, parentTypeSymbol.Definition);

            var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            var ilgen = ctorBuilder.GetILGenerator();


            ConstructorInfo parentCtor;
            var ctorSymbol = (Symbol.CtorSymbol)parentTypeSymbol.Symbols[SymbolKey.Ctor];
            if (ctorSymbol != null)
                parentCtor = ctorSymbol.Definition;
            else
            {
                var obj = typeof(object);
                parentCtor = obj.GetConstructor(Type.EmptyTypes);
            }

            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, parentCtor);

            typeSymbol.Definition = typeBuilder;
            typeSymbol.Symbols.Add(SymbolKey.Ctor, new Symbol.CtorSymbol() { Definition = ctorBuilder });
        }

        private void defineGlobals(ModuleBuilder moduleBuilder, string assemblyName)
        {
            glob = moduleBuilder.DefineType($"{assemblyName}.Global", TypeAttributes.Public | TypeAttributes.Sealed, typeof(Runtime.GlobalBase));

            var ctorBuilder = glob.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            var ilgen = ctorBuilder.GetILGenerator();
            var globalBaseCtor = (Symbol.CtorSymbol)((Symbol.TypeSymbol)globalSymbols[new SymbolKey(SymbolType.Type, "/@")]).Symbols[SymbolKey.Ctor];

            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, globalBaseCtor.Definition);

            globalSymbols.Add(SymbolKey.Ctor, new Symbol.CtorSymbol()
            {
                DefinitionBuilder = ctorBuilder
            });


            foreach (var item in globalSymbols)
            {
                switch (item.Key.Type)
                {
                    case SymbolType.Proc:
                        defineProc(glob, item.Key.Name, (Symbol.ProcSymbol)item.Value, true);
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
                return; // parent_type is special compile time var, se we don't define it.

            var baseVarSymbol = varSymbol.FindVarBaseDefinition(globalSymbols);
            varSymbol.Stage = Symbol.DefinitionStage.SIGNATURE;
            if(baseVarSymbol == varSymbol)
            {
                Type varType = varSymbol.Var.TypeOf();
                bool canhaveDefaultValue = varSymbol.Var.CanHaveDefaultValue();
                FieldAttributes attributes = varSymbol.Var.DetermineBestFieldAttributes();
                if (canhaveDefaultValue)
                    attributes |= FieldAttributes.HasDefault;
                var fieldBuilder = typeBuilder.DefineField(name, varType, attributes);
                if (canhaveDefaultValue)
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
                    varSymbol.Stage = Symbol.DefinitionStage.FULLY_DEFINED;
                }
                varSymbol.Definition = fieldBuilder;
            } else
            {
                varSymbol.Definition = baseVarSymbol.Definition; // We borow definition from initial var declaration place
            }
            
        }

        private void defineProc(TypeBuilder typeBuilder, string name, Symbol.ProcSymbol procSymbol, bool isStatic = false)
        {
            // ToDo: determine return type



            Console.WriteLine(procSymbol);

            //var procBuilder = typeBuilder.DefineMethod(name, MethodAttributes.Public);
            //procSymbol.Definition = procBuilder;
        }

        private void finalizeTypes(ModuleBuilder mb, string assemblyName)
        {
            var globCtor = (Symbol.CtorSymbol)globalSymbols[SymbolKey.Ctor];
            var globilemitter = globCtor.DefinitionBuilder.GetILGenerator();
            globilemitter.Emit(OpCodes.Ret);

            foreach (var item in globalSymbols.Where(kv => kv.Key.Type == SymbolType.Type).Select(kv => (Symbol.TypeSymbol)kv.Value).SelectMany(i => i.Symbols)
                .Where(kv => kv.Key.Type == SymbolType.Ctor).Select(kv => (Symbol.CtorSymbol)kv.Value).Where(i => i.DefinitionBuilder != null))
            {
                var ctorIlEmitter = item.DefinitionBuilder.GetILGenerator();
                ctorIlEmitter.Emit(OpCodes.Ret);

                item.Stage = Symbol.DefinitionStage.FULLY_DEFINED;
            }


            glob.CreateType();
            foreach (var item in globalSymbols.Where(kv => kv.Key.Type == SymbolType.Type).Select(kv => (Symbol.TypeSymbol)kv.Value))
            {
                item.DefinitionBuilder?.CreateType();
                item.Stage = Symbol.DefinitionStage.FULLY_DEFINED;
            }
        }


        private static string GenerateClassNameFromPath(string path, string assemblyName)
        {
            var sb = new StringBuilder(assemblyName);
            sb.Append('.');
            foreach (var segment in path.Split('/', StringSplitOptions.RemoveEmptyEntries))
            {
                /*sb.Append(char.ToUpper(segment[0]));
                sb.Append(segment.Substring(1));*/
                sb.Append(segment);
                sb.Append('_');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
