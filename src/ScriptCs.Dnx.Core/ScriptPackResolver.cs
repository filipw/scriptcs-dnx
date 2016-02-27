using System.Collections.Generic;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class ScriptPackResolver : IScriptPackResolver
    {
        private IEnumerable<IScriptPack> _scriptPacks;

        public ScriptPackResolver(IEnumerable<IScriptPack> scriptPacks)
        {
            _scriptPacks = scriptPacks;
        }

        public IEnumerable<IScriptPack> GetPacks()
        {
            return _scriptPacks;
        }
    }
}