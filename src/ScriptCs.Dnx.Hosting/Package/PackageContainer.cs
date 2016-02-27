using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using NuGet.Versioning;
using ScriptCs.Dnx.Contracts;
using System.Linq;
using System.Xml.Linq;
using NuGet.Packaging;

namespace ScriptCs.Dnx.Hosting.Package
{
    public class PackageContainer : IPackageContainer
    {
        private const string DotNetFramework = ".NETFramework";

        private const string DotNetPortable = ".NETPortable";

        private readonly IFileSystem _fileSystem;

        private readonly ILog _logger;

        public PackageContainer(IFileSystem fileSystem, ILogProvider logProvider)
        {
            if (fileSystem == null) throw new ArgumentNullException(nameof(fileSystem));
            if (logProvider == null) throw new ArgumentNullException(nameof(logProvider));

            _fileSystem = fileSystem;
            _logger = logProvider.ForCurrentType();
        }

        public void CreatePackageFile()
        {
            //todo: implement
            //var packagesFile = Path.Combine(_fileSystem.CurrentDirectory, _fileSystem.PackagesFile);
            //var packageReferenceFile = new PackageReferenceFile(packagesFile);

            //var packagesFolder = Path.Combine(_fileSystem.CurrentDirectory, _fileSystem.PackagesFolder);
            //var repository = new LocalPackageRepository(packagesFolder);

            //var newestPackages = repository.GetPackages().GroupBy(p => p.Id)
            //    .Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault());

            //if (!newestPackages.Any())
            //{
            //    _logger.Info("No packages found!");
            //    return;
            //}

            //_logger.InfoFormat("{0} {1}...", (File.Exists(packagesFile) ? "Updating" : "Creating"), _fileSystem.PackagesFile);

            //foreach (var package in newestPackages)
            //{
            //    var newestFramework = GetNewestSupportedFramework(package);

            //    if (!packageReferenceFile.EntryExists(package.Id, package.Version))
            //    {
            //        packageReferenceFile.AddEntry(package.Id, package.Version, package.DevelopmentDependency, newestFramework);

            //        if (newestFramework == null)
            //        {
            //            _logger.InfoFormat("Added {0} (v{1}) to {2}", package.Id, package.Version, _fileSystem.PackagesFile);
            //        }
            //        else
            //        {
            //            _logger.InfoFormat("Added {0} (v{1}, .NET {2}) to {3}", package.Id, package.Version, newestFramework.Version, _fileSystem.PackagesFile);
            //        }

            //        continue;
            //    }

            //    _logger.InfoFormat("Skipped {0} because it already exists.", package.Id);
            //}

            //_logger.InfoFormat("Successfully {0} {1}.", (File.Exists(packagesFile) ? "updated" : "created"), _fileSystem.PackagesFile);
        }

        public IPackageObject FindPackage(string path, IPackageReference packageRef)
        {
            if (packageRef == null) throw new ArgumentNullException(nameof(packageRef));

            return null;
            //todo
            ////var repository = new LocalPackageRepository(path);
            //var folderReader = new PackageFolderReader(path);
            //var nupkgFiles = folderReader.GetFiles().Where(x => Path.GetExtension(x).ToLowerInvariant() == ".nupkg");

            //var result = new List<string>();

            //foreach (var nupkg in nupkgFiles)
            //{
            //    var stream = folderReader.GetStream(nupkg);
            //    folderReader.GetPackageDependencies()
            //    var packageReader = new PackageReader(stream);

            //    var identity = packageReader.GetIdentity();
            //    if (identity.Id == packageRef.PackageId && identity.Version.Version == packageRef.Version)
            //    {
            //        return new PackageObject(identity, );
            //    }

            //    var package = packageRef.Version != null && !(packageRef.Version.Major == 0 && packageRef.Version.Minor == 0)
            //        ? repository.FindPackage(packageRef.PackageId, new SemanticVersion(packageRef.Version, packageRef.SpecialVersion), true, true)
            //        : repository.FindPackage(packageRef.PackageId);
            //    //var packagesConfigReference = contents.FirstOrDefault(x => x.PackageIdentity.Id == identity.Id && x.PackageIdentity.Version == identity.Version);

            //    //if (packagesConfigReference == null)
            //    //{
            //    //    break;
            //    //}

            //    //var packageContents = packageReader.GetLibItems().Where(x => compatibilityProvider.IsCompatible(x.TargetFramework, packagesConfigReference.TargetFramework)).
            //    //    SelectMany(x => x.Items.Where(i => Path.GetExtension(i).ToLowerInvariant() == ".dll"));

            //    //result.AddRange(packageContents);
            //}

            //var package = packageRef.Version != null && !(packageRef.Version.Major == 0 && packageRef.Version.Minor == 0)
            //    ? repository.FindPackage(packageRef.PackageId, new SemanticVersion(packageRef.Version, packageRef.SpecialVersion), true, true)
            //    : repository.FindPackage(packageRef.PackageId);

            //return package == null ? null : new PackageObject(package, packageRef.FrameworkName);
        }

        public IEnumerable<IPackageReference> FindReferences(string path)
        {
            var packagesConfig = XDocument.Parse(File.ReadAllText(path));
            var reader = new PackagesConfigReader(packagesConfig);

            var references = reader.GetPackages().ToList();
            if (references.Any())
            {
                foreach (var packageReference in references)
                {
                    yield return new ScriptCs.Dnx.Core.PackageReference(
                        packageReference.PackageIdentity.Id,
                        new FrameworkName(packageReference.TargetFramework.DotNetFrameworkName),
                        packageReference.PackageIdentity.Version.ToString());
                }

                yield break;
            }

            //todo:
            // No packages.config, check packages folder
            //var packagesFolder = Path.Combine(_fileSystem.GetWorkingDirectory(path), _fileSystem.PackagesFolder);
            //if (!_fileSystem.DirectoryExists(packagesFolder))
            //{
            //    yield break;
            //}

            //var repository = new LocalPackageRepository(packagesFolder);

            //var arbitraryPackages = repository.GetPackages();
            //if (!arbitraryPackages.Any())
            //{
            //    yield break;
            //}

            //foreach (var arbitraryPackage in arbitraryPackages)
            //{
            //    var newestFramework = GetNewestSupportedFramework(arbitraryPackage)
            //                          ?? VersionUtility.EmptyFramework;

            //    yield return new PackageReference(
            //        arbitraryPackage.Id,
            //        newestFramework,
            //        arbitraryPackage.Version.Version,
            //        arbitraryPackage.Version.SpecialVersion);
            //}
        }

        //private static FrameworkName GetNewestSupportedFramework(IPackage packageMetadata)
        //{
        //    return packageMetadata.GetSupportedFrameworks()
        //        .Where(IsValidFramework)
        //        .OrderByDescending(x => x.Version)
        //        .FirstOrDefault();
        //}

        private static bool IsValidFramework(FrameworkName frameworkName)
        {
            return frameworkName.Identifier == DotNetFramework
                   || (frameworkName.Identifier == DotNetPortable
                       && frameworkName.Profile.Split('+').Any(IsValidProfile));
        }

        private static bool IsValidProfile(string profile)
        {
            return profile == "net40" || profile == "net45";
        }
    }
}