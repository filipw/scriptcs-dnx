using System;
using System.Collections.Generic;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx
{
    public class ScriptCsArgs
    {
        public bool Repl { get; set; }

        public string ScriptName { get; set; }

        public bool Help { get; set; }

        public bool Debug { get; set; }

        public bool Cache { get; set; }

        public LogLevel? LogLevel { get; set; }

        public string Install { get; set; }

        public bool Global { get; set; }

        public bool Save { get; set; }

        public bool Clean { get; set; }

        public bool AllowPreRelease { get; set; }

        public bool Version { get; set; }

        public bool Watch { get; set; }

        public string Modules { get; set; }

        public string Config { get; set; }

        public string PackageVersion { get; set; }

        public string Output { get; set; }

        public static ScriptCsArgs Parse(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            var curatedArgs = new List<string>();
            string implicitPackageVersion = null;
            for (var index = 0; index < args.Length; ++index)
            {
                if (index < args.Length - 2 &&
                    (string.Equals(args[index], "-install", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(args[index], "-i", StringComparison.OrdinalIgnoreCase)) &&
                    !args[index + 1].StartsWith("-", StringComparison.Ordinal) &&
                    !args[index + 2].StartsWith("-", StringComparison.Ordinal))
                {
                    curatedArgs.Add(args[index]);
                    curatedArgs.Add(args[index + 1]);
                    implicitPackageVersion = args[index + 2];
                    index += 2;
                }
                else
                {
                    curatedArgs.Add(args[index]);
                }
            }

            //todo! right now we hardcode scriptrunning
            //var scriptCsArgs = Args.Parse<ScriptCsArgs>(curatedArgs.ToArray());
            //scriptCsArgs.PackageVersion = scriptCsArgs.PackageVersion ?? implicitPackageVersion;

            if (args.Length == 0) return new ScriptCsArgs {Repl = true};

            return new ScriptCsArgs
            {
                ScriptName = args[0]
            };
        }
    }
}