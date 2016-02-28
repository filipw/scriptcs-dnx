using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class ClearCommand : IReplCommand
    {
        private readonly IConsole _console;

        public string Description => "Clears the console window.";

        public ClearCommand(IConsole console)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));

            _console = console;
        }

        public string CommandName => "clear";

        public object Execute(IRepl repl, object[] args)
        {
            _console.Clear();
            return null;
        }
    }
}