using DMNet.SpacemanDMM.AST;
using System;
using System.Text.Json;
using System.Linq;
using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using DMNet.SpacemanDMM;
using DMNet.Compiler;
using System.Collections.Generic;
using System.Reflection;

namespace DMNet
{
    class Program
    {
        static void Main(string[] args)
        {
           var parser = new Parser(@"C:\Projektai\DMBase\DMBase.dme");
           // var parser = new Parser(@"C:\Projektai\Aurora.3\aurorastation.dme");
           Console.WriteLine(parser);


            var c = new Compiler.Compiler(parser);
            c.BuildSymbolData();
            Console.WriteLine(c);

            var Assm = c.Emit("DMBase");
            var generator = new Lokad.ILPack.AssemblyGenerator();

            var refrencedAssemblies = new List<Assembly>();

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            refrencedAssemblies.Add(loadedAssemblies.Where(a => a.GetName().Name == "System.Runtime").First());

            generator.GenerateAssembly(Assm, "DMBase.dll");

        }
    }
}
