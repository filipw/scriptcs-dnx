using System;
using System.IO;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core.ReplCommands
{
    public class CdCommand : IReplCommand
    {
        public string Description => "Changes the working directory to the path provided.";

        public string CommandName => "cd";

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null) throw new ArgumentNullException(nameof(repl));

            if (args == null || args.Length == 0)
            {
                return null;
            }

            var path = args[0].ToString();

            repl.FileSystem.CurrentDirectory = Path.GetFullPath(Path.Combine(repl.FileSystem.CurrentDirectory, path));

            return null;
        }
    }
}