using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class ResetCommand : IReplCommand
    {
        public string Description => "Resets the REPL state. All local variables and member definitions are cleared.";

        public string CommandName => "reset";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            repl.Reset();
            return null;
        }
    }
}