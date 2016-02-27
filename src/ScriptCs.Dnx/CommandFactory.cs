﻿using System;
using ScriptCs.Dnx.Command;
using ScriptCs.Dnx.Contracts;
using ScriptCs.Dnx.Hosting;

namespace ScriptCs.Dnx
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
            var scriptServices = _scriptServicesBuilder.Build();

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