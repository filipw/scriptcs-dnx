using System;
using System.Linq;
using System.Collections.Generic;

namespace ScriptCs.Dnx.Contracts
{
    public static class FileSystemExtensions
    {
        public static IEnumerable<string> EnumerateBinaries(this IFileSystem fileSystem, string path)
        {
            return fileSystem.EnumerateFiles(path, "*.dll").Union(fileSystem.EnumerateFiles(path, "*.exe")).Where(f => f.ToLowerInvariant() != "scriptcs.exe");
        }
    }
}