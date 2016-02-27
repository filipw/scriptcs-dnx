using System;
using System.Collections.Generic;
using ScriptCs.Dnx.Contracts;
using ScriptCs.Dnx.Core;

namespace ScriptCs.Dnx
{
    public class Program
    {
        public void Main(string[] args)
        {
            var dir = @"C:\code\dummy\";
            var scriptFile = @"C:\code\dummy\foo.csx";

            var fs = new FileSystem {CurrentDirectory = dir};

            var filePreProcessor = new FilePreProcessor(fs, new List<ILineProcessor>
            {
                new LoadLineProcessor(fs), new ReferenceLineProcessor(fs), new UsingLineProcessor()
            });
            var result = filePreProcessor.ProcessFile(scriptFile);

            foreach (var loadedScript in result.LoadedScripts)
            {
                Console.WriteLine(loadedScript);
            }

            foreach (var reference in result.References)
            {
                Console.WriteLine(reference);
            }

            foreach (var ns in result.Namespaces)
            {
                Console.WriteLine(ns);
            }

            //var assemblyResolver = new AssemblyResolver(fs);
            //var binaries = assemblyResolver.GetAssemblyPaths(fs.CurrentDirectory);
            //foreach (var binary in binaries)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine(binary);
            //    Console.WriteLine("-----");
            //}

            Console.ReadLine();
        }
    }
}
