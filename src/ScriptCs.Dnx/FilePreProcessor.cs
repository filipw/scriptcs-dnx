using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx
{
    public class FilePreProcessor : IFilePreProcessor
    {
        private readonly IEnumerable<ILineProcessor> _lineProcessors;

        private readonly IFileSystem _fileSystem;

        public FilePreProcessor(IFileSystem fileSystem, IEnumerable<ILineProcessor> lineProcessors)
        {
            _fileSystem = fileSystem;
            _lineProcessors = lineProcessors;
        }

        public virtual FilePreProcessorResult ProcessFile(string path)
        {
            return Process(context => ParseFile(path, context));
        }

        public virtual FilePreProcessorResult ProcessScript(string script)
        {
            var scriptLines = _fileSystem.SplitLines(script).ToList();
            return Process(context => ParseScript(scriptLines, context));
        }

        protected virtual FilePreProcessorResult Process(Action<FileParserContext> parseAction)
        {
            var context = new FileParserContext();
            parseAction(context);

            var code = GenerateCode(context);

            return new FilePreProcessorResult
            {
                Namespaces = context.Namespaces,
                LoadedScripts = context.LoadedScripts,
                References = context.References,
                Code = code
            };
        }

        protected virtual string GenerateCode(FileParserContext context)
        {
            return string.Join(_fileSystem.NewLine, context.BodyLines);
        }

        public virtual void ParseFile(string path, FileParserContext context)
        {
            var fullPath = _fileSystem.GetFullPath(path);
            var filename = Path.GetFileName(path);

            if (context.LoadedScripts.Contains(fullPath))
            {
                return;
            }

            // Add script to loaded collection before parsing to avoid loop.
            context.LoadedScripts.Add(fullPath);

            var scriptLines = _fileSystem.ReadFileLines(fullPath).ToList();

            InsertLineDirective(fullPath, scriptLines);
            InDirectory(fullPath, () => ParseScript(scriptLines, context));
        }

        public virtual void ParseScript(List<string> scriptLines, FileParserContext context)
        {
            var codeIndex = scriptLines.FindIndex(IsNonDirectiveLine);

            for (var index = 0; index < scriptLines.Count; index++)
            {
                var line = scriptLines[index];
                var isBeforeCode = index < codeIndex || codeIndex < 0;

                var wasProcessed = _lineProcessors.Any(x => x.ProcessLine(this, context, line, isBeforeCode));

                if (!wasProcessed)
                {
                    context.BodyLines.Add(line);
                }
            }
        }

        protected virtual void InsertLineDirective(string path, List<string> fileLines)
        {
            var bodyIndex = fileLines.FindIndex(line => IsNonDirectiveLine(line) && !IsUsingLine(line));
            if (bodyIndex == -1)
            {
                return;
            }

            var directiveLine = string.Format("#line {0} \"{1}\"", bodyIndex + 1, path);
            fileLines.Insert(bodyIndex, directiveLine);
        }

        private void InDirectory(string path, Action action)
        {
            var oldCurrentDirectory = _fileSystem.CurrentDirectory;
            _fileSystem.CurrentDirectory = _fileSystem.GetWorkingDirectory(path);

            action();

            _fileSystem.CurrentDirectory = oldCurrentDirectory;
        }

        private bool IsNonDirectiveLine(string line)
        {
            var directiveLineProcessors =
                _lineProcessors.OfType<IDirectiveLineProcessor>();

            return line.Trim() != string.Empty && !directiveLineProcessors.Any(lp => lp.Matches(line));
        }

        private static bool IsUsingLine(string line)
        {
            return line.TrimStart(' ').StartsWith("using ") && !line.Contains("{") && line.Contains(";");
        }
    }
}
