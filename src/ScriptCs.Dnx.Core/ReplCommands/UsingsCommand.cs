using System;
using System.Linq;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class UsingsCommand : IReplCommand
    {
        public string Description => "Displays a list of namespaces imported into REPL context.";

        public string CommandName => "usings";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            var namespaces = repl.Namespaces;

            if (repl.ScriptPackSession == null || repl.ScriptPackSession.Namespaces == null || !repl.ScriptPackSession.Namespaces.Any())
                return namespaces;

            return namespaces.Union(repl.ScriptPackSession.Namespaces).OrderBy(x => x);
        }
    }
}