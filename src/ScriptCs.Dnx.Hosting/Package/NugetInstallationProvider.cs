using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Versioning;
using ScriptCs.Dnx.Contracts;
using ScriptCs.Dnx.Core;
using System.Linq;

namespace ScriptCs.Dnx.Hosting.Package
{
     class NewAssemblyResolver : IAssemblyResolver
    {
        private readonly Dictionary<string, List<string>> _assemblyPathCache = new Dictionary<string, List<string>>();
        private readonly IFileSystem _fileSystem;

        public NewAssemblyResolver(IFileSystem fileSystem)
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

    public class NugetInstallationProvider : IInstallationProvider
    {
        
        /* todo: this whole class needs a rework
        private readonly IFileSystem _fileSystem;
        private readonly ILog _logger;
        private PackageManager _manager;
        private IEnumerable<string> _repositoryUrls;

        private static readonly Version EmptyVersion = new Version();

        public NugetInstallationProvider(IFileSystem fileSystem, ILogProvider logProvider)
        {
            Guard.AgainstNullArgument("fileSystem", fileSystem);
            Guard.AgainstNullArgument("logProvider", logProvider);

            _fileSystem = fileSystem;
            _logger = logProvider.ForCurrentType();
        }

        public void Initialize()
        {
            var path = Path.Combine(_fileSystem.CurrentDirectory, _fileSystem.PackagesFolder);
            _repositoryUrls = GetRepositorySources(path);
            var remoteRepository = new AggregateRepository(PackageRepositoryFactory.Default, _repositoryUrls, true);
            _manager = new PackageManager(remoteRepository, path);
        }

        public IEnumerable<string> GetRepositorySources(string path)
        {
            var configFileSystem = new PhysicalFileSystem(path);

            ISettings settings;
            var localNuGetConfigFile = Path.Combine(_fileSystem.CurrentDirectory, _fileSystem.NugetFile);
            if (_fileSystem.FileExists(localNuGetConfigFile))
            {
                settings = Settings.LoadDefaultSettings(configFileSystem, localNuGetConfigFile, null);
            }
            else
            {
                settings = Settings.LoadDefaultSettings(configFileSystem, null, new NugetMachineWideSettings());
            }

            if (settings == null)
            {
                return new[] { Constants.DefaultRepositoryUrl };
            }

            var sourceProvider = new PackageSourceProvider(settings);
            var sources = sourceProvider.LoadPackageSources().Where(i => i.IsEnabled == true);

            if (sources == null || !sources.Any())
            {
                return new[] { Constants.DefaultRepositoryUrl };
            }

            return sources.Select(i => i.Source);
        }

        public void InstallPackage(IPackageReference packageId, bool allowPreRelease = false)
        {
            Guard.AgainstNullArgument("packageId", packageId);

            var version = GetVersion(packageId);
            var packageName = packageId.PackageId + " " + (version == null ? string.Empty : packageId.Version.ToString());
            _manager.InstallPackage(packageId.PackageId, version, allowPrereleaseVersions: allowPreRelease, ignoreDependencies: false);
            _logger.Info("Installed: " + packageName);
        }

        private static SemanticVersion GetVersion(IPackageReference packageReference)
        {
            return packageReference.Version == EmptyVersion ? null : new SemanticVersion(packageReference.Version, packageReference.SpecialVersion);
        }

        public bool IsInstalled(IPackageReference packageReference, bool allowPreRelease = false)
        {
            Guard.AgainstNullArgument("packageReference", packageReference);

            var version = GetVersion(packageReference);
            return _manager.LocalRepository.FindPackage(packageReference.PackageId, version, allowPreRelease, allowUnlisted: false) != null;
        }
        */
        public IEnumerable<string> GetRepositorySources(string path)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public bool IsInstalled(IPackageReference packageId, bool allowPreRelease = false)
        {
            throw new NotImplementedException();
        }

        public void InstallPackage(IPackageReference packageId, bool allowPreRelease = false)
        {
            throw new NotImplementedException();
        }
    }
}