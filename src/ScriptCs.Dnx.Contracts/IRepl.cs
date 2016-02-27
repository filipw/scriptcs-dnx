using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IRepl : IScriptExecutor
    {
        Dictionary<string, IReplCommand> Commands { get; }

        string Buffer { get; }
    }
}