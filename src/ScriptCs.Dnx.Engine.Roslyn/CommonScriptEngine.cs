using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using ScriptCs.Dnx.Contracts;
using ScriptCs.Dnx.Core;

namespace ScriptCs.Dnx.Engine.Roslyn
{
    public abstract class CommonScriptEngine : IScriptEngine
    {
        protected ScriptOptions ScriptOptions;
        protected InteractiveAssemblyLoader Loader;

        private readonly IScriptHostFactory _scriptHostFactory;
        private readonly ILog _log;

        public const string SessionKey = "Session";

        protected CommonScriptEngine(IScriptHostFactory scriptHostFactory, ILogProvider logProvider)
        {
            if (logProvider == null) throw new ArgumentNullException(nameof(logProvider));
            ScriptOptions = ScriptOptions.Default.AddReferences(typeof (Object).GetTypeInfo().Assembly);
            _scriptHostFactory = scriptHostFactory;
            _log = logProvider.ForCurrentType();
            Loader = new InteractiveAssemblyLoader();
        }

        public string BaseDirectory
        {
            //todo: this is probably broken
            get { return ((SourceFileResolver)ScriptOptions.SourceResolver).BaseDirectory; }
            set { ScriptOptions = ScriptOptions.WithSourceResolver(new SourceFileResolver(new string[0], value)); }
        }

        public string CacheDirectory { get; set; }

        public string FileName { get; set; }

        public ScriptResult Execute(string code, string[] scriptArgs, AssemblyReferences references, IEnumerable<string> namespaces, ScriptPackSession scriptPackSession)
        {
            if (scriptPackSession == null)
            {
                throw new ArgumentNullException("scriptPackSession");
            }

            if (references == null)
            {
                throw new ArgumentNullException("references");
            }

            _log.Debug("Starting to create execution components");
            _log.Debug("Creating script host");

            var executionReferences = new AssemblyReferences(references.Assemblies, references.Paths);
            executionReferences.Union(scriptPackSession.References);

            ScriptResult scriptResult;
            SessionState<ScriptState> sessionState;

            var isFirstExecution = !scriptPackSession.State.ContainsKey(SessionKey);

            if (isFirstExecution)
            {
                var host = _scriptHostFactory.CreateScriptHost(new ScriptPackManager(scriptPackSession.Contexts), scriptArgs);

                ScriptLibraryWrapper.SetHost(host);
                _log.Debug("Creating session");

                var hostType = host.GetType();
                Loader.RegisterDependency(hostType.GetTypeInfo().Assembly);

                //ScriptOptions = ScriptOptions.AddReferences(typeof(Console).GetTypeInfo().Assembly);
                ScriptOptions = ScriptOptions.AddReferences(typeof(Object).GetTypeInfo().Assembly);

                var allNamespaces = namespaces.Union(scriptPackSession.Namespaces).Distinct();

                foreach (var reference in executionReferences.Paths)
                {
                    _log.DebugFormat("Adding reference to {0}", reference);
                    ScriptOptions = ScriptOptions.AddReferences(reference);
                }

                foreach (var assembly in executionReferences.Assemblies)
                {
                    _log.DebugFormat("Adding reference to {0}", assembly.FullName);
                    ScriptOptions = ScriptOptions.AddReferences(assembly);
                }

                foreach (var @namespace in allNamespaces)
                {
                    _log.DebugFormat("Importing namespace {0}", @namespace);
                    ScriptOptions = ScriptOptions.AddImports(@namespace);
                }

                sessionState = new SessionState<ScriptState> { References = executionReferences, Namespaces = new HashSet<string>(allNamespaces) };
                scriptPackSession.State[SessionKey] = sessionState;

                scriptResult = Execute(code, host, sessionState);
            }
            else
            {
                _log.Debug("Reusing existing session");
                sessionState = (SessionState<ScriptState>)scriptPackSession.State[SessionKey];

                if (sessionState.References == null)
                {
                    sessionState.References = new AssemblyReferences();
                }

                if (sessionState.Namespaces == null)
                {
                    sessionState.Namespaces = new HashSet<string>();
                }

                var newReferences = executionReferences.Except(sessionState.References);

                foreach (var reference in newReferences.Paths)
                {
                    _log.DebugFormat("Adding reference to {0}", reference);
                    ScriptOptions = ScriptOptions.AddReferences(reference);
                    sessionState.References = sessionState.References.Union(new[] { reference });
                }

                foreach (var assembly in newReferences.Assemblies)
                {
                    _log.DebugFormat("Adding reference to {0}", assembly.FullName);
                    ScriptOptions = ScriptOptions.AddReferences(assembly);
                    sessionState.References = sessionState.References.Union(new[] { assembly });
                }

                var newNamespaces = namespaces.Except(sessionState.Namespaces);

                foreach (var @namespace in newNamespaces)
                {
                    _log.DebugFormat("Importing namespace {0}", @namespace);
                    ScriptOptions = ScriptOptions.AddImports(@namespace);
                    sessionState.Namespaces.Add(@namespace);
                }

                if (string.IsNullOrWhiteSpace(code))
                {
                    return ScriptResult.Empty;
                }

                scriptResult = Execute(code, sessionState.Session, sessionState);
            }

            return scriptResult;

            //todo handle namespace failures
            //https://github.com/dotnet/roslyn/issues/1012
        }

        protected virtual ScriptResult Execute(string code, object globals, SessionState<ScriptState> sessionState)
        {
            try
            {
                _log.Debug("Starting execution");

                var script = CSharpScript.Create(code, ScriptOptions, globals.GetType(), Loader);
                sessionState.Session = sessionState.Session == null ? 
                    script.RunAsync(globals).Result : 
                    sessionState.Session.ContinueWithAsync(code).Result;

                _log.Debug("Finished execution");
                return new ScriptResult(returnValue: sessionState.Session.ReturnValue);
            }
            catch (AggregateException ex)
            {
                return new ScriptResult(executionException: ex.InnerException);
            }
            catch (CompilationErrorException ex)
            {
                return new ScriptResult(compilationException: ex);
            }
            catch (Exception ex)
            {
                return new ScriptResult(executionException: ex);
            }
        }
    }
}
