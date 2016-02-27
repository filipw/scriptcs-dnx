using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IPackageAssemblyResolver
    {
        void SavePackages();
        IEnumerable<IPackageReference> GetPackages(string workingDirectory);
        IEnumerable<string> GetAssemblyNames(string workingDirectory);
    }
}