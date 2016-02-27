using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
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
            //todo: move this up cause it won't work the REPL
            using (var loader = new InteractiveAssemblyLoader())
            {
                //todo: don't hardcode IScriptHost
                loader.RegisterDependency(typeof(IScriptHost).GetTypeInfo().Assembly);
                var script = CSharpScript.Create(code, ScriptOptions, typeof(IScriptHost), loader);
                return script.RunAsync(globals).Result;
            }
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