using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Engine.Roslyn
{
    public class CSharpScriptEngine : CommonScriptEngine
    {
        public CSharpScriptEngine(IScriptHostFactory scriptHostFactory, ILogProvider logProvider) : base(scriptHostFactory, logProvider)
        {
        }


        protected override ScriptState GetScriptState(string code, object globals)
        {
            //todo: async all the things?
            return CSharpScript.RunAsync(code, ScriptOptions, globals).Result;
        }

        protected bool IsCompleteSubmission(string code)
        {
            //invalid REPL command
            if (code.StartsWith(":"))
            {
                return true;
            }

            var options = new CSharpParseOptions(LanguageVersion.CSharp6, DocumentationMode.Parse,
                SourceCodeKind.Script, null);

            var syntaxTree = SyntaxFactory.ParseSyntaxTree(code, options: options);
            return SyntaxFactory.IsCompleteSubmission(syntaxTree);
        }
    }
}