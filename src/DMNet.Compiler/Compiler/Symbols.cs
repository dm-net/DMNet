using DMNet.SpacemanDMM.AST;
using System.Linq;
using DMType = DMNet.SpacemanDMM.Type;

namespace DMNet.Compiler
{
    public partial class Compiler
    {

        public void BuildSymbolData()
        {
            foreach (DMType type in _parser.Types)
            {
                _symBuildWalk(type);
            }
        }

        private void _symBuildWalk(DMType type)
        {
            bool isRoot = type.IsRoot;
            SymbolTable targetSymbolTable;
            Symbol.TypeSymbol symbol;
            if (type.IsRoot)
            {
                targetSymbolTable = globalSymbols;
            }
            else
            {
                symbol = new Symbol.TypeSymbol(type);
                targetSymbolTable = symbol.Symbols;
                globalSymbols.Add(new SymbolKey(SymbolType.Type, type.Path), symbol);
            }
            foreach (var variable in type.Vars)
            {
                _symBuildWalk(variable, targetSymbolTable);
            }
            foreach (var proc in type.Procs)
            {
                _symBuildWalk(proc, targetSymbolTable);
            }
        }

        private void _symBuildWalk(TypeProc proc, SymbolTable targetSymbolTable)
        {
            Symbol.ProcSymbol symbol = new Symbol.ProcSymbol(proc);
            targetSymbolTable.Add(new SymbolKey(SymbolType.Proc, proc.Name), symbol);
            TypeProcValue procValue = proc.Values.Last();

            if (procValue.Code is Code.Present code)
            {
                _symBuildWalk(code.Block, symbol.Symbols);
            }
        }

        private void _symBuildWalk(Block block, SymbolTable targetSymbolTable)
        {
            foreach (var stmt in block.Value)
            {
                var statement = stmt.Value;
                Symbol.StatementSymbol sym;
                switch (statement)
                {
                    case Statement.While whileStmt:
                        sym = new Symbol.StatementSymbol(whileStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(whileStmt.Block, sym.Symbols);
                        break;
                    case Statement.DoWhile doWhileStmt:
                        sym = new Symbol.StatementSymbol(doWhileStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(doWhileStmt.Block, sym.Symbols);
                        break;
                    case Statement.If ifStmt:
                        {
                            foreach (var item in ifStmt.Arms)
                            {
                                sym = new Symbol.StatementSymbol(ifStmt);
                                targetSymbolTable.AddAnonimousScope(sym);
                                _symBuildWalk(item.Item2, sym.Symbols);
                            }
                            if (ifStmt.Else != null)
                            {
                                sym = new Symbol.StatementSymbol(ifStmt);
                                targetSymbolTable.AddAnonimousScope(sym);
                                _symBuildWalk(ifStmt.Else, sym.Symbols);
                            }
                        }
                        break;
                    case Statement.ForLoop forLoopStmt:
                        sym = new Symbol.StatementSymbol(forLoopStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(forLoopStmt.Block, sym.Symbols);
                        break;
                    case Statement.ForList forListStmt:
                        sym = new Symbol.StatementSymbol(forListStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(forListStmt.Block, sym.Symbols);
                        break;
                    case Statement.ForRange forRangeStmt:
                        sym = new Symbol.StatementSymbol(forRangeStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(forRangeStmt.Block, sym.Symbols);
                        break;
                    case Statement.Spawn spawnStmt:
                        sym = new Symbol.StatementSymbol(spawnStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(spawnStmt.Block, sym.Symbols);
                        break;
                    case Statement.Switch switchStmt:
                        {
                            foreach (var item in switchStmt.Cases)
                            {
                                sym = new Symbol.StatementSymbol(switchStmt);
                                targetSymbolTable.AddAnonimousScope(sym);
                                _symBuildWalk(item.Item2, sym.Symbols);
                            }
                            if (switchStmt.Default != null)
                            {
                                sym = new Symbol.StatementSymbol(switchStmt);
                                targetSymbolTable.AddAnonimousScope(sym);
                                _symBuildWalk(switchStmt.Default, sym.Symbols);
                            }
                        }
                        break;
                    case Statement.TryCatch tryCatchStmt:
                        sym = new Symbol.StatementSymbol(tryCatchStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(tryCatchStmt.Try, sym.Symbols);
                        sym = new Symbol.StatementSymbol(tryCatchStmt);
                        targetSymbolTable.AddAnonimousScope(sym);
                        _symBuildWalk(tryCatchStmt.Catch, sym.Symbols);
                        break;
                    case Statement.Var varStmt:
                        {
                            var varSymbol = new Symbol.VarSymbol(varStmt.Value);
                            targetSymbolTable.Add(new SymbolKey(SymbolType.Var, varStmt.Value.Name), varSymbol);
                        }
                        break;
                    case Statement.Vars varsStmt:
                        foreach (var varStmt in varsStmt.Value)
                        {
                            var varSymbol = new Symbol.VarSymbol(varStmt);
                            targetSymbolTable.Add(new SymbolKey(SymbolType.Var, varStmt.Name), varSymbol);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void _symBuildWalk(TypeVar variable, SymbolTable targetSymbolTable)
        {
            Symbol.VarSymbol symbol = new Symbol.VarSymbol(variable);
            targetSymbolTable.Add(new SymbolKey(SymbolType.Var, variable.Name), symbol);
        }
    }
}
