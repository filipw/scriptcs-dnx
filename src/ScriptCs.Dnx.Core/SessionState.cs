using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class SessionState<T>
    {
        public T Session { get; set; }

        public AssemblyReferences References { get; set; }

        public HashSet<string> Namespaces { get; set; }
    }

    //todo: check, I think this is net451 atm only
    //[Serializable]
}
