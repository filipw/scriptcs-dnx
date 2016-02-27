using System;
using System.Collections.Generic;
using System.Linq;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Hosting.Package
{
    public class PackageInstaller : IPackageInstaller
    {
        private readonly IInstallationProvider _installer;
        private readonly ILog _logger;

        public PackageInstaller(IInstallationProvider installer, ILogProvider logProvider)
        {
            if (installer == null) throw new ArgumentNullException(nameof(installer));
            if (logProvider == null) throw new ArgumentNullException(nameof(logProvider));

            _installer = installer;
            _logger = logProvider.ForCurrentType();
        }

        public void InstallPackages(IEnumerable<IPackageReference> packageIds, bool allowPreRelease = false)
        {
            if (packageIds == null) throw new ArgumentNullException(nameof(packageIds));

            packageIds = packageIds.Where(packageId => !_installer.IsInstalled(packageId, allowPreRelease)).ToList();

            if (!packageIds.Any())
            {
                _logger.Info("Nothing to install.");
                return;
            }

            var exceptions = new List<Exception>();
            foreach (var packageId in packageIds)
            {
                try
                {
                    _installer.InstallPackage(packageId, allowPreRelease);
                }
                catch (Exception ex)
                {
                    _logger.ErrorException("Error installing package.", ex);
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}