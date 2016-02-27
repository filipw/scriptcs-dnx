using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptEnvironment
    {
        IReadOnlyList<string> ScriptArgs { get; }
    }
}