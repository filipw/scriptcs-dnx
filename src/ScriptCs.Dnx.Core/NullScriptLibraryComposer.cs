using System.Text;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class NullScriptLibraryComposer : IScriptLibraryComposer
    {
        public void Compose(string workingDirectory, StringBuilder builder = null)
        {
        }

        public string ScriptLibrariesFile
        {
            get { return string.Empty; }
        }
    }
}