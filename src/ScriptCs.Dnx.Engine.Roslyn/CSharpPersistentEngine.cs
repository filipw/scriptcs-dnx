using System;
using System.IO;
using System.Reflection;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Engine.Roslyn
{
    public class CSharpPersistentEngine : CSharpScriptCompilerEngine
    {
        private readonly IFileSystem _fileSystem;
        private const string RoslynAssemblyNameCharacter = "ℛ";
        private readonly ILog _log;

        public CSharpPersistentEngine(IScriptHostFactory scriptHostFactory, ILogProvider logProvider, IFileSystem fileSystem)
            : base(scriptHostFactory, logProvider)
        {
            if (logProvider == null) throw new ArgumentNullException(nameof(logProvider));
            _log = logProvider.ForCurrentType();
            _fileSystem = fileSystem;
        }

        protected override bool ShouldCompile()
        {
            var dllPath = GetDllTargetPath();

            return !_fileSystem.FileExists(dllPath);
        }

        protected override Assembly LoadAssembly(byte[] exeBytes, byte[] pdbBytes)
        {
            throw new NotImplementedException("todo, app domains need to be rethought");
            _log.DebugFormat("Writing assembly to {0}.", FileName);

            if (!_fileSystem.DirectoryExists(CacheDirectory))
            {
                _fileSystem.CreateDirectory(CacheDirectory, true);
            }

            var dllPath = GetDllTargetPath();
            _fileSystem.WriteAllBytes(dllPath, exeBytes);

            _log.DebugFormat("Loading assembly {0}.", dllPath);

            // the assembly is automatically loaded into the AppDomain when compiled
            // just need to find and return it
            //return AppDomain.CurrentDomain.GetAssemblies().LastOrDefault(x => x.FullName.StartsWith(RoslynAssemblyNameCharacter));
        }

        protected override Assembly LoadAssemblyFromCache()
        {
            throw new NotImplementedException("todo, app domains need to be rethought");
            var dllPath = GetDllTargetPath();
            //return Assembly.LoadFrom(dllPath);
        }

        private string GetDllTargetPath()
        {
            var dllName = FileName.Replace(Path.GetExtension(FileName), ".dll");
            var dllPath = Path.Combine(CacheDirectory, dllName);
            return dllPath;
        }
    }
}