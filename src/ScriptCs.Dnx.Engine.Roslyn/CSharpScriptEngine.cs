using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Engine.Roslyn
{
    public class CSharpScriptEngine : CommonScriptEngine
    {
        private readonly CSharpParseOptions _parseOptions;

        public CSharpScriptEngine(IScriptHostFactory scriptHostFactory, ILogProvider logProvider) : base(scriptHostFactory, logProvider)
        {
            _parseOptions = new CSharpParseOptions(LanguageVersion.CSharp6, DocumentationMode.Parse, SourceCodeKind.Script, null);
        }

        protected bool IsCompleteSubmission(string code)
        {
            //invalid REPL command
            if (code.StartsWith(":"))
            {
                return true;
            }

            var syntaxTree = SyntaxFactory.ParseSyntaxTree(code, options: _parseOptions);
            return SyntaxFactory.IsCompleteSubmission(syntaxTree);
        }
    }
}