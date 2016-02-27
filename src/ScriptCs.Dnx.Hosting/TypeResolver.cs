using System;

namespace ScriptCs.Dnx.Hosting
{
    public class TypeResolver : ITypeResolver
    {
        public Type ResolveType(string type)
        {
            return Type.GetType(type);
        }
    }
}