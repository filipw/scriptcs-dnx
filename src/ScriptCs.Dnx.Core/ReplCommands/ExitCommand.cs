using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class ExitCommand : IReplCommand
    {
        private readonly IConsole _console;

        public string Description => "Exits the REPL";

        public string CommandName => "exit";

        public ExitCommand(IConsole console)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));

            _console = console;
        }

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            var response = string.Empty;
            var responseIsValid = false;

            while (!responseIsValid)
            {
                _console.WriteLine("Are you sure you wish to exit? (y/n):");
                response = _console.ReadLine().ToLowerInvariant();
                responseIsValid = response == "y" || response == "n";
            }

            if (response == "y")
            {
                repl.Terminate();
            }

            return null;
        }
    }
}