using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IPackageInstaller
    {
        void InstallPackages(IEnumerable<IPackageReference> packageIds, bool allowPreRelease = false);
    }
}