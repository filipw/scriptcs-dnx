using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace ScriptCs.Dnx.Contracts
{
    public static class FileSystemExtensions
    {
        public static IEnumerable<string> EnumerateBinaries(
            this IFileSystem fileSystem, string path, SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (fileSystem == null) throw new ArgumentNullException(nameof(fileSystem));

            return fileSystem.EnumerateFiles(path, "*.dll", searchOption)
                .Union(fileSystem.EnumerateFiles(path, "*.exe", searchOption))
                .Where(f => !f.Equals("scriptcs.exe", StringComparison.OrdinalIgnoreCase));
        }
    }
}