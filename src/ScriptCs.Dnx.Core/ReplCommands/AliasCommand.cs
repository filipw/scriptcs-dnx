using System;
using System.Globalization;
using System.Linq;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class AliasCommand : IReplCommand
    {
        private readonly IConsole _console;

        public AliasCommand(IConsole console)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));

            _console = console;
        }

        public string Description => "Allows you to alias a command with a custom name";

        public string CommandName => "alias";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            if (args == null || args.Length != 2)
            {
                _console.WriteLine("You must specifiy the command name and alias, e.g. :alias \"clear\" \"cls\"");
                return null;
            }

            var commandName = args[0].ToString();
            var alias = args[1].ToString();

            if (repl.Commands.Any(x => string.Equals(x.Key, alias, StringComparison.OrdinalIgnoreCase)))
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "\"{0}\" cannot be used as an alias since it is the name of an existing command.",
                    alias);

                _console.WriteLine(message);
                return null;
            }

            IReplCommand command;
            if (!repl.Commands.TryGetValue(commandName, out command))
            {
                var message = string.Format(
                    CultureInfo.InvariantCulture, "There is no command named or aliased \"{0}\".", alias);

                _console.WriteLine(message);
                return null;
            }

            repl.Commands[alias] = command;
            _console.WriteLine(string.Format("Aliased \"{0}\" as \"{1}\".", commandName, alias));

            return null;
        }
    }
}