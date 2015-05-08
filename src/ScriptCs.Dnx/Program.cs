using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx
{
    public class Program
    {
        public void Main(string[] args)
        {
            var dir = @"C:\Users\filip\Documents\dev\dummy\";
            var scriptFile = @"C:\Users\filip\Documents\dev\dummy\foo.csx";

            var fs = new FileSystem {CurrentDirectory = dir};

            var filePreProcessor = new FilePreProcessor(fs, new List<ILineProcessor> {new LoadLineProcessor(fs), new ReferenceLineProcessor(fs)});
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

            var assemblyResolver = new AssemblyResolver(fs);
            var binaries = assemblyResolver.GetAssemblyPaths(fs.CurrentDirectory);
            foreach (var binary in binaries)
            {
                Console.WriteLine();
                Console.WriteLine(binary);
                Console.WriteLine("-----");
            }

            Console.ReadLine();
        }
    }
}
