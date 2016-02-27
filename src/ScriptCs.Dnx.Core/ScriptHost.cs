using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class ScriptHost : IScriptHost
    {
        private readonly IScriptPackManager _scriptPackManager;

        public ScriptHost(IScriptPackManager scriptPackManager, ScriptEnvironment environment)
        {
            if (scriptPackManager == null) throw new ArgumentNullException(nameof(scriptPackManager));

            _scriptPackManager = scriptPackManager;
            Env = environment;
        }

        public IScriptEnvironment Env { get; private set; }

        public T Require<T>() where T : IScriptPackContext
        {
            return _scriptPackManager.Get<T>();
        }
    }
}