using System;
using ScriptCs.Dnx.Contracts;
using System.Linq;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class ReferencesCommand : IReplCommand
    {
        public string CommandName => "references";

        public string Description => "Displays a list of assemblies referenced from the REPL context.";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            return repl.References?.Assemblies.Select(x => x.FullName).Union(repl.References.Paths);
        }
    }
}