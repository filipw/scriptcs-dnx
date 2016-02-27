using System;

namespace ScriptCs.Dnx.Hosting
{
    public interface ITypeResolver
    {
        Type ResolveType(string typeName);
    }
}