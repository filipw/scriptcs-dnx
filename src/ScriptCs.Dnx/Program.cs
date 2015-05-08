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
            var file = @"C:\Users\filip\Documents\dev\dummy\scriptcs_packages.config";
            var folder = @"C:\Users\filip\Documents\dev\dummy\scriptcs_packages";
            var scriptFile = @"C:\Users\filip\Documents\dev\dummy\foo.csx";

            var compatibilityProvider = new DefaultCompatibilityProvider();
            var folderReader = new PackageFolderReader(folder);
            var nupkgFiles = folderReader.GetFiles().Where(x => Path.GetExtension(x).ToLowerInvariant() == ".nupkg");

            var packagesConfig = XDocument.Parse(File.ReadAllText(file));

            var reader = new PackagesConfigReader(packagesConfig);
            var contents = reader.GetPackages();

            foreach (var nupkg in nupkgFiles)
            {
                var stream = folderReader.GetStream(nupkg);
                var packageReader = new PackageReader(stream);

                var identity = packageReader.GetIdentity();
                var packagesConfigReference = contents.FirstOrDefault(x => x.PackageIdentity.Id == identity.Id && x.PackageIdentity.Version == identity.Version);

                if (packagesConfigReference == null)
                {
                    break;
                }

                var packageContents = packageReader.GetLibItems().Where(x => compatibilityProvider.IsCompatible(x.TargetFramework, packagesConfigReference.TargetFramework)).
                    SelectMany(x => x.Items.Where(i => Path.GetExtension(i).ToLowerInvariant() == ".dll"));

                foreach (var packageContent in packageContents)
                {
                    Console.WriteLine();
                    Console.WriteLine(packageContent);
                    Console.WriteLine("-----");
                }
            }

            var fs = new FileSystem();
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

            Console.ReadLine();
        }
    }
}
