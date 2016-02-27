using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Hosting
{
    public class ModuleLoader : IModuleLoader
    {
        public void Load(IModuleConfiguration config, string[] modulePackagesPaths, string hostBin, string extension,
            params string[] moduleNames)
        {
            //todo: implement later
        }
    }
}