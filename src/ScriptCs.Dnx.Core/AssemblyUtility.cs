using System;
using System.Reflection;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    //todo: not sure if this even works
    public class AssemblyUtility : IAssemblyUtility
    {
        public bool IsManagedAssembly(string path)
        {
            try
            {
                new AssemblyName(path);
                return true;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }

        public Assembly LoadFile(string path)
        {
            return Assembly.Load(GetAssemblyName(path));
        }

        public Assembly Load(AssemblyName assemblyRef)
        {
            return Assembly.Load(assemblyRef);
        }

        public AssemblyName GetAssemblyName(string path)
        {
            return new AssemblyName(path);
        }
    }
}