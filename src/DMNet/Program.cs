using DMNet.SpacemanDMM.AST;
using System;
using System.Text.Json;
using System.Linq;
using Dahomey.Cbor;
using Dahomey.Cbor.ObjectModel;
using DMNet.SpacemanDMM;

namespace DMNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new ManagedParser(@"C:\Projektai\DMBase\DMBase.dme");
            //var parser = new ManagedParser(@"C:\Projektai\Aurora.3\aurorastation.dme");
            Console.WriteLine(parser);

            var rootType = parser.RootType;
            //Console.WriteLine(rootType);
            //Console.WriteLine(rootType.Path);
            //Console.WriteLine(rootType.IsRoot);
            //var cborObject = Cbor.Deserialize<CborArray>(rootType.GetVarsCbor());
            //Console.WriteLine(BitConverter.ToString(rootType.GetVarsCbor()).Replace('-', ' '));
            //var rootVars = rootType.GetVars();




            var types = parser.GetTypes();

            //var myDatum = types.First(t => t.Path.Contains("myDatum"));

            foreach (var type in types)
            {
                Console.WriteLine($"{type.Path}");
                var vars = type.GetVars();
                var procs = type.GetProcs();
                Console.WriteLine($"{procs}");
            }

            //var things = rootType.GetVars();
            //Console.WriteLine(rootType.GetVarsJson());

            //var types = objTree.GetTypes();
            //foreach (var child in types)
            //{
            //    Console.WriteLine(child);
            //    Console.WriteLine(child.Path);
            //}


            Console.ReadKey();
        }
    }
}
