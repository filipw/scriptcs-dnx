using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class CwdCommand : IReplCommand
    {
        private readonly IConsole _console;

        public CwdCommand(IConsole console)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));

            _console = console;
        }

        public string Description => "Displays the current working directory.";

        public string CommandName => "cwd";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            var dir = repl.FileSystem.CurrentDirectory;

            var originalColor = _console.ForegroundColor;
            _console.ForegroundColor = ConsoleColor.Yellow;
            try
            {
                _console.WriteLine(dir);
            }
            finally
            {
                _console.ForegroundColor = originalColor;
            }

            return null;
        }
    }
}