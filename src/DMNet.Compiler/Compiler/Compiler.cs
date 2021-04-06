using DMNet.SpacemanDMM;
using DMNet.SpacemanDMM.AST;
using System;
using System.Linq;
using DMType = DMNet.SpacemanDMM.Type;

namespace DMNet.Compiler
{
    public partial class Compiler
    {

        private SymbolTable globalSymbols;
        private Parser _parser;

        
        public Compiler(Parser parser)
        {
            _parser = parser;
            globalSymbols = new SymbolTable(null);
        }
        
    }
}
