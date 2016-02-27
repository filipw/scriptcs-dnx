using System.Text;

namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptLibraryComposer
    {
        void Compose(string workingDirectory, StringBuilder builder = null);

        string ScriptLibrariesFile { get; }
    }
}