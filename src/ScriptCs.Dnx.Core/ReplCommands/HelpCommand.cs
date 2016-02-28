using System;
using ScriptCs.Dnx.Contracts;
using System.Linq;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class HelpCommand : IReplCommand
    {
        private readonly IConsole _console;

        public HelpCommand(IConsole console)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));

            _console = console;
        }

        public string Description => "Shows this help.";

        public string CommandName => "help";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            _console.WriteLine("The following commands are available in the REPL:");
            foreach (var command in repl.Commands.OrderBy(x => x.Key))
            {
                _console.WriteLine(string.Format(":{0,-15}{1,10}", command.Key, command.Value.Description));
            }

            return null;
        }
    }
}