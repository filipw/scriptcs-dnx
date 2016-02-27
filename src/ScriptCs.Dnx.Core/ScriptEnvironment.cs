using System.Collections.Generic;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class ScriptEnvironment : IScriptEnvironment
    {
        public ScriptEnvironment(string[] scriptArgs)
        {
            ScriptArgs = scriptArgs;
        }

        public IReadOnlyList<string> ScriptArgs { get; private set; }
    }
}