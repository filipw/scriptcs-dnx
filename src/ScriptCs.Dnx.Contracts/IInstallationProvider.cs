using System;
using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IInstallationProvider
    {
        IEnumerable<string> GetRepositorySources(string path);
        void Initialize();
        bool IsInstalled(IPackageReference packageId, bool allowPreRelease = false);
        void InstallPackage(IPackageReference packageId, bool allowPreRelease = false);
    }

    public interface IModuleConfiguration : IServiceOverrides<IModuleConfiguration>
    {
        bool Cache { get; }

        string ScriptName { get; }

        bool IsRepl { get; }

        LogLevel LogLevel { get; }

        bool Debug { get; }

        IDictionary<Type, object> Overrides { get; }
    }
}