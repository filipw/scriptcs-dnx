﻿using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public interface ILoadLineProcessor : ILineProcessor
    {
    }

    public class LoadLineProcessor : DirectiveLineProcessor, ILoadLineProcessor
    {
        private readonly IFileSystem _fileSystem;

        public LoadLineProcessor(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        protected override string DirectiveName
        {
            get { return "load"; }
        }

        protected override BehaviorAfterCode BehaviorAfterCode
        {
            get { return BehaviorAfterCode.Throw; }
        }

        protected override bool ProcessLine(IFileParser parser, FileParserContext context, string line)
        {
            var argument = GetDirectiveArgument(line);
            var filePath = Environment.ExpandEnvironmentVariables(argument);

            var fullPath = _fileSystem.GetFullPath(filePath);
            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                parser.ParseFile(fullPath, context);
            }

            return true;
        }
    }
}