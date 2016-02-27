using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class AssemblyResolver
    {
        private readonly Dictionary<string, List<string>> _assemblyPathCache = new Dictionary<string, List<string>>();
        private readonly IFileSystem _fileSystem;

        public AssemblyResolver(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IEnumerable<string> GetAssemblyPaths(string path, bool binariesOnly = false)
        {
            List<string> assemblies;
            if (!_assemblyPathCache.TryGetValue(path, out assemblies))
            {
                assemblies = GetPackageAssemblyNames(path).Union(GetBinAssemblyPaths(path)).ToList();
                _assemblyPathCache.Add(path, assemblies);
            }

            return binariesOnly ? assemblies.Where(m =>
                    Path.GetExtension(m).ToLowerInvariant() == ".dll" ||
                    Path.GetExtension(m).ToLowerInvariant() == ".exe")
                : assemblies.ToArray();
        }

        private IEnumerable<string> GetBinAssemblyPaths(string path)
        {
            var binFolder = Path.Combine(path, _fileSystem.BinFolder);
            if (!_fileSystem.DirectoryExists(binFolder))
            {
                yield break;
            }

            foreach (var assembly in _fileSystem.EnumerateBinaries(binFolder))
            {
                yield return assembly;
            }
        }

        private IEnumerable<string> GetPackageAssemblyNames(string path)
        {
            var compatibilityProvider = new DefaultCompatibilityProvider();
            var folderReader = new PackageFolderReader(Path.Combine(path, _fileSystem.PackagesFolder));
            var nupkgFiles = folderReader.GetFiles().Where(x => Path.GetExtension(x).ToLowerInvariant() == ".nupkg");

            var packagesConfig = XDocument.Parse(File.ReadAllText(Path.Combine(path, _fileSystem.PackagesFile)));

            var reader = new PackagesConfigReader(packagesConfig);
            var contents = reader.GetPackages();

            var result = new List<string>();

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

                result.AddRange(packageContents);
            }

            return result;
        }
    }
}