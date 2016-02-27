using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ScriptCs.Dnx.Contracts
{
    public class AssemblyReferences
    {
        private readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();
        private readonly Dictionary<string, string> _paths = new Dictionary<string, string>();

        public AssemblyReferences()
            : this((IEnumerable<string>) Enumerable.Empty<string>())
        {
        }

        public AssemblyReferences(IEnumerable<Assembly> assemblies)
            : this(assemblies, Enumerable.Empty<string>())
        {
        }

        public AssemblyReferences(IEnumerable<string> paths)
            : this(Enumerable.Empty<Assembly>(), paths)
        {
        }

        public AssemblyReferences(IEnumerable<Assembly> assemblies, IEnumerable<string> paths)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            if (paths == null) throw new ArgumentNullException(nameof(paths));

            foreach (var assembly in assemblies.Where(assembly => assembly != null))
            {
                var name = assembly.GetName().Name;
                if (!_assemblies.ContainsKey(name))
                {
                    _assemblies.Add(name, assembly);
                }
            }

            foreach (var path in paths)
            {
                var name = Path.GetFileName(path);
                if (name == null)
                {
                    continue;
                }

                if (name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                    name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    name = Path.GetFileNameWithoutExtension(name);
                }

                if (!_paths.ContainsKey(name) && !_assemblies.ContainsKey(name))
                {
                    _paths.Add(name, path);
                }
            }
        }

        public IEnumerable<Assembly> Assemblies
        {
            get { return _assemblies.Values.ToArray(); }
        }

        public IEnumerable<string> Paths
        {
            get { return _paths.Values.ToArray(); }
        }

        public AssemblyReferences Union(AssemblyReferences references)
        {
            if (references == null) throw new ArgumentNullException(nameof(references));

            return new AssemblyReferences(Assemblies.Union(references.Assemblies), Paths.Union(references.Paths));
        }

        public AssemblyReferences Union(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            return new AssemblyReferences(Assemblies.Union(assemblies), Paths);
        }

        public AssemblyReferences Union(IEnumerable<string> paths)
        {
            if (paths == null) throw new ArgumentNullException(nameof(paths));

            return new AssemblyReferences(Assemblies, Paths.Union(paths));
        }

        public AssemblyReferences Except(AssemblyReferences references)
        {
            if (references == null) throw new ArgumentNullException(nameof(references));

            return new AssemblyReferences(Assemblies.Except(references.Assemblies), Paths.Except(references.Paths));
        }

        public AssemblyReferences Except(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            return new AssemblyReferences(Assemblies.Except(assemblies), Paths);
        }

        public AssemblyReferences Except(IEnumerable<string> paths)
        {
            if (paths == null) throw new ArgumentNullException(nameof(paths));

            return new AssemblyReferences(Assemblies, Paths.Except(paths));
        }
    }
}