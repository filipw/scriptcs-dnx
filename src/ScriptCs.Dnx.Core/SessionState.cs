﻿using System.Collections.Generic;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class SessionState<T>
    {
        public T Session { get; set; }

        public AssemblyReferences References { get; set; }

        public HashSet<string> Namespaces { get; set; }
    }
}
