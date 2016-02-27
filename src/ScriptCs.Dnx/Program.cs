using System;
using System.IO;
using ScriptCs.Dnx.Core;
using System.Linq;

namespace ScriptCs.Dnx
{
    public class Program
    {
        public int Main(string[] args)
        {

            var nonScriptArgs = args.TakeWhile(arg => arg != "--").ToArray();
            var scriptArgs = args.Skip(nonScriptArgs.Length + 1).ToArray();

            ScriptCsArgs commandArgs;
            try
            {
                commandArgs = ScriptCsArgs.Parse(nonScriptArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }

            if (commandArgs.Config != null && !File.Exists(commandArgs.Config))
            {
                Console.WriteLine("The specified config file does not exist.");
                return 1;
            }

            return Application.Run(Config.Create(commandArgs), scriptArgs);
        }
    }
}
