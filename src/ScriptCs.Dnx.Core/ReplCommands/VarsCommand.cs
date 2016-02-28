using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class VarsCommand : IReplCommand
    {
        public string CommandName => "vars";

        public string Description => "Displays a list of variables defined within the REPL, along with their types and values.";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            var replEngine = repl.ScriptEngine as IReplEngine;
            return replEngine?.GetLocalVariables(repl.ScriptPackSession);
        }
    }
}