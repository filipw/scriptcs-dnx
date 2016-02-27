using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptPackResolver
    {
        IEnumerable<IScriptPack> GetPacks();
    }
}