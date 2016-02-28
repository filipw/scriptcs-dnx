using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;
using ScriptCs.Dnx.Contracts;
using ScriptCs.Dnx.Hosting;

namespace ScriptCs.Dnx.Command
{
    public class CommandFactory
    {
        private readonly IScriptServicesBuilder _scriptServicesBuilder;
        private readonly IInitializationServices _initializationServices;
        private readonly IFileSystem _fileSystem;

        public CommandFactory(IScriptServicesBuilder scriptServicesBuilder)
        {
            if (scriptServicesBuilder == null) throw new ArgumentNullException(nameof(scriptServicesBuilder));

            _scriptServicesBuilder = scriptServicesBuilder;
            _initializationServices = _scriptServicesBuilder.InitializationServices;
            _fileSystem = _initializationServices.GetFileSystem();

            if (_fileSystem.PackagesFile == null)
            {
                throw new ArgumentException(
                    "The file system provided by the initialization services provided by the script services builder has a null packages file.",
                    "scriptServicesBuilder");
            }

            if (_fileSystem.PackagesFolder == null)
            {
                throw new ArgumentException(
                    "The file system provided by the initialization services provided by the script services builder has a null package folder.",
                    "scriptServicesBuilder");
            }
        }

        //todo: this needs to import other commands
        public ICommand CreateCommand(Config config, string[] scriptArgs)
        {
            if (scriptArgs == null) throw new ArgumentNullException(nameof(scriptArgs));

            var libs =
                DnxPlatformServices.Default.LibraryManager.GetLibraries()
                    .Where(x => x.Name.ToLower().Contains("scriptcs"))
                    .SelectMany(x => x.Assemblies).Select(x => Assembly.Load(x));
            var scriptServices = _scriptServicesBuilder.Build(libs);

            if (config.Repl)
            {
                var explicitReplCommand = new ExecuteReplCommand(
                    config.ScriptName,
                    scriptArgs,
                    scriptServices.FileSystem,
                    scriptServices.ScriptPackResolver,
                    scriptServices.Repl,
                    scriptServices.LogProvider,
                    scriptServices.Console,
                    scriptServices.AssemblyResolver,
                    scriptServices.FileSystemMigrator,
                    scriptServices.ScriptLibraryComposer);

                return explicitReplCommand;
            }

            return new ExecuteScriptCommand(
                config.ScriptName,
                scriptArgs,
                scriptServices.FileSystem,
                scriptServices.Executor,
                scriptServices.ScriptPackResolver,
                scriptServices.LogProvider,
                scriptServices.AssemblyResolver,
                scriptServices.FileSystemMigrator,
                scriptServices.ScriptLibraryComposer);
        }
    }
}