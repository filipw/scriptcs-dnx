using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Hosting
{
    public interface IInitializationServices
    {
        IAssemblyResolver GetAssemblyResolver();

        IModuleLoader GetModuleLoader();

        IFileSystem GetFileSystem();

        IInstallationProvider GetInstallationProvider();

        IPackageAssemblyResolver GetPackageAssemblyResolver();

        IPackageInstaller GetPackageInstaller();

        ILogProvider LogProvider { get; }

        //todo: this needs serious overhaul
        //IAppDomainAssemblyResolver GetAppDomainAssemblyResolver();

        IAssemblyUtility GetAssemblyUtility();
    }
}