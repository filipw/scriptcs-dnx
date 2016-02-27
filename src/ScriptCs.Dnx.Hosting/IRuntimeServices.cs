using ScriptCs.Dnx.Core;

namespace ScriptCs.Dnx.Hosting
{
    public interface IRuntimeServices
    {
        ScriptServices GetScriptServices();
    }
}