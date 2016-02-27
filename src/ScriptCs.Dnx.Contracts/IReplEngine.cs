using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IReplEngine : IScriptEngine
    {
        ICollection<string> GetLocalVariables(ScriptPackSession scriptPackSession);
    }
}