using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class ScriptHostFactory : IScriptHostFactory
    {
        public IScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            return new ScriptHost(scriptPackManager, new ScriptEnvironment(scriptArgs));
        }
    }
}